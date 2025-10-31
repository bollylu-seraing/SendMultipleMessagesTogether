using SendMultipleMessagesTogether.Process;
using SendMultipleMessagesTogether.Support;
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

namespace SendMultipleMessagesTogether {
  [ComVisible(true)]
  public class RibbonSMA : Office.IRibbonExtensibility {
    private Office.IRibbonUI ribbon;

    private const string RIBBON_ID = "Microsoft.Outlook.Explorer";
    private const string TAB_ID = "TabMail";
    private const string GROUP_ID = "SendMultipleMessagesTogetherGroup";
    private const string BUTTON_ID = "ProcessMessagesButton";

    private const string INDICATEUR_BUTTON_ID = "Indicateur";
    private const string EDIT_INDICATEUR_PARAMETERS_BUTTON_ID = "IndicateurParam";
    private ILogger Logger => ThisAddIn.Logger;

    #region --- Constructor ---------------------------------------------------
    public RibbonSMA() {

    }
    #endregion ----------------------------------------------------------------

    #region IRibbonExtensibility Members

    public string GetCustomUI(string ribbonID) {
      return GetResourceText("SendMultipleMessagesTogether.RibbonSMA.xml");
    }

    #endregion

    #region Ribbon Callbacks
    //Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226

    public void Ribbon_Load(Office.IRibbonUI ribbonUI) {
      this.ribbon = ribbonUI;
    }

    public void ProcessMessages_Click(Office.IRibbonControl control) {
      IProcess Process = new TProcess(Globals.ThisAddIn.Application, ThisAddIn.Logger, ThisAddIn.Parameters);
      Process.Execute();
    }

    public void EditParameters_Click(Office.IRibbonControl control) {
      using (Form1 ParametersForm = new Form1(ThisAddIn.Parameters)) {
        DialogResult Result =  ParametersForm.ShowDialog();
        if (Result == DialogResult.OK) {
          ParametersForm.NewParameters.Save();
          ThisAddIn.Parameters.Read();
          Logger.LogInfo("Reading new parameters ...");
          Logger.LogInfo($"  Recipient: {ThisAddIn.Parameters.Recipient.WithQuotes()}");
          Logger.LogInfo($"  Prefix: {ThisAddIn.Parameters.Prefix.WithQuotes()}");
          Logger.LogInfo($"  Category: {ThisAddIn.Parameters.Category.WithQuotes()}");
          Logger.LogInfo($"  LogFilename: {ThisAddIn.Parameters.LogFilename.WithQuotes()}");
        }
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
      // Logger.LogInfo($"GetImage called for {control.Id}");
      switch (control.Id) {
        case INDICATEUR_BUTTON_ID:
          //Logger.LogInfo("Returning image for Indicateur");
          return SendMultipleMessagesTogether.Properties.Resources.letter;
        case EDIT_INDICATEUR_PARAMETERS_BUTTON_ID:
          //Logger.LogInfo("Returning image for IndicateurParams");
          return SendMultipleMessagesTogether.Properties.Resources.parameters.ToBitmap();
        default:
          Logger.LogError($"Unknown control Id: {control.Id}");
          return null;

      }
      
    }
    #endregion
  }
}
