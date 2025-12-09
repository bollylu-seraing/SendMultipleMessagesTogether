using Microsoft.Office.Interop.Outlook;

using SMA.Process;
using SMA.Support;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace SMA {
  [ComVisible(true)]
  public class RibbonSMA : Office.IRibbonExtensibility {
    private Office.IRibbonUI ribbon;

    private const string RIBBON_ID = "Microsoft.Outlook.Explorer";
    private const string TAB_ID = "TabMail";
    private const string GROUP_ID = "SMAGroup";

    private const string INDICATEUR_BUTTON_ID = "Indicateur";
    private const string EDIT_INDICATEUR_PARAMETERS_BUTTON_ID = "IndicateurParam";
    private const string CLEANUP_SENT_ITEMS_BUTTON_ID = "CleanupSentItems";
    private const string INDICATEUR_LOG_ID = "IndicateurLog";

    private ILogger Logger => ThisAddIn.Logger;

    // Be tolerant: don't throw during ribbon load
    private Outlook.Application Application => Globals.ThisAddIn?.Application;
    private Explorer ActiveExplorer => Application?.ActiveExplorer();
    private MAPIFolder SentMailFolder => Application?.Session == null ? null : Application.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);

    #region --- Constructor ---------------------------------------------------
    public RibbonSMA() {
      // avoid any logging/modal UI here — ribbon may be created before ThisAddIn_Startup
    }
    #endregion ----------------------------------------------------------------

    #region IRibbonExtensibility Members

    public string GetCustomUI(string ribbonID) {
      return GetResourceText("SMA.RibbonSMA.xml");
    }

    #endregion

    #region Ribbon Callbacks
    //Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226

    public void Ribbon_Load(Office.IRibbonUI ribbonUI) {
      this.ribbon = ribbonUI;
      // avoid modal logging here
    }

    public void SendToIndicator_Click(Office.IRibbonControl control) {
      IProcess Process = new TProcess(Globals.ThisAddIn.Application, ThisAddIn.Logger, ThisAddIn.Parameters);
      Process.SendToIndicator();
    }

    public void CleanupSentItems_Click(Office.IRibbonControl control) {
      IProcess Process = new TProcess(Globals.ThisAddIn.Application, ThisAddIn.Logger, ThisAddIn.Parameters);
      int RemovedItemsCount = Process.CleanupSentItems();
      MessageBox.Show(
        $"Nettoyage terminé. {RemovedItemsCount} élément(s) supprimé(s) du dossier Eléments envoyés.",
        "Nettoyage des éléments envoyés",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
    }

    public void EditParameters_Click(Office.IRibbonControl control) {
      using (FormParams ParametersForm = new FormParams(ThisAddIn.Parameters)) {
        DialogResult Result = ParametersForm.ShowDialog();
        if (Result == DialogResult.OK) {
          Logger.LogInfo("Saving new parameters from form ...");
          ParametersForm.NewParameters.Save();
          Logger.LogInfo("Reading new parameters ...");
          ThisAddIn.Parameters.Read();
          foreach (string LineItem in ThisAddIn.Parameters
            .ToString()
            .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
            .Where(x => !string.IsNullOrWhiteSpace(x))) {
            Logger.LogInfo(LineItem);
          }
        }
      }
    }

    public void ViewLog_Click(Office.IRibbonControl control) {
      using (FormLog LogForm = new FormLog(ThisAddIn.Logger)) {
        LogForm.ShowDialog();
      }
    }

    #endregion

    #region Helpers

    private static string GetResourceText(string resourceName) {
      Assembly asm = Assembly.GetExecutingAssembly();
      string[] resourceNames = asm.GetManifestResourceNames();
      for (int i = 0; i < resourceNames.Length; ++i) {
        if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0) {
          using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i]))) {
            if (resourceReader != null) {
              return resourceReader.ReadToEnd();
            }
          }
        }
      }
      return null;
    }

    public Image GetImage(Office.IRibbonControl control) {
      // avoid logging that shows modal dialogs during ribbon load
      switch (control.Id) {
        case INDICATEUR_BUTTON_ID:
          return SMA.Properties.Resources.letter;
        case EDIT_INDICATEUR_PARAMETERS_BUTTON_ID:
          return SMA.Properties.Resources.parameters.ToBitmap();
        case CLEANUP_SENT_ITEMS_BUTTON_ID:
          return SMA.Properties.Resources.parameters.ToBitmap();
        case INDICATEUR_LOG_ID:
          return SMA.Properties.Resources.parameters.ToBitmap();
        default:
          // don't show message boxes during ribbon load; just return null
          return null;
      }
    }

    public bool GetTabVisibility(Office.IRibbonControl control) {
      try {
        var app = Globals.ThisAddIn?.Application;
        if (app == null) {
          return false;
        }
        var explorer = app.ActiveExplorer();
        if (explorer == null) {
          return false;
        }
        var folder = explorer.CurrentFolder;
        if (folder == null) {
          return false;
        }
        return folder.DefaultItemType == Outlook.OlItemType.olMailItem;
      } catch {
        // swallow exceptions in UI callbacks — prefer returning not visible
        return false;
      }
    }

    #endregion
  }
}
