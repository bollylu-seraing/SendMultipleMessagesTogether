using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

using SendMultipleMessagesTogether.Process;

using Office = Microsoft.Office.Core;

namespace SendMultipleMessagesTogether {
  [ComVisible(true)]
  public class RibbonSMA : Office.IRibbonExtensibility {
    private Office.IRibbonUI ribbon;

    public RibbonSMA() {

    }

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

    public Image GetImageLetter(Office.IRibbonControl control) {
      return SendMultipleMessagesTogether.Properties.Resources.letter;
    }
    #endregion
  }
}
