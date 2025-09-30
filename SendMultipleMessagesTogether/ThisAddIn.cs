using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

using SendMultipleMessagesTogether.Support;

using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace SendMultipleMessagesTogether {
  public partial class ThisAddIn {

    public const string DEFAULT_APPLICATION_NAME = "SendMultipleMessagesTogether";

    public const string PARAMETERS_FILENAME = "ApplicationParameters.conf";
    public static string PARAMETERS_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DEFAULT_APPLICATION_NAME);

    public static string ApplicationPath { get; private set; } = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    public static string ApplicationParameters { get; private set; } = Path.Combine(PARAMETERS_PATH, PARAMETERS_FILENAME);

    public static IParameters Parameters { get; private set; }

    protected override Office.IRibbonExtensibility CreateRibbonExtensibilityObject() {
      Parameters = new TParametersRegistry();
      Parameters.Logger?.LogInfo("Reading base parameters from registry.");
      return Globals.Factory.GetRibbonFactory().CreateRibbonManager(
        new Microsoft.Office.Tools.Ribbon.IRibbonExtension[] {
          new RibbonSMMAA()
        }
      );
    }

    private void ThisAddIn_Startup(object sender, System.EventArgs e) {
      Parameters = new TParametersRegistry();
      Parameters.Logger?.LogInfo("Reading parameters from registry.");
      if (!Parameters.Read()) {
        Parameters = new TParametersConf(new TMessageBoxLogger(), ApplicationParameters);
        Parameters.Logger?.LogInfo($"Reading parameters from configuration file {ApplicationParameters.WithQuotes()}.");
        if (!Parameters.Read()) {
          Parameters.Logger?.LogWarning("Using default parameters.");
        }
      }
    }


    #region VSTO generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup() {
      this.Startup += new System.EventHandler(ThisAddIn_Startup);
    }

    #endregion
  }
}
