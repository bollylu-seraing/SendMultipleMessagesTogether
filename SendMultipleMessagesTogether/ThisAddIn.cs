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
    public static Version APPLICATION_VERSION = new Version(1, 2);

    public const string PARAMETERS_FILENAME = "ApplicationParameters.conf";
    public static string PARAMETERS_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DEFAULT_APPLICATION_NAME);

    public static ILogger DEFAULT_LOGGER => new TMessageBoxLogger();
    public static ILogger Logger => Parameters?.Logger ?? DEFAULT_LOGGER;

    public static string ApplicationPath { get; private set; } = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    public static string ApplicationParameters { get; private set; } = Path.Combine(PARAMETERS_PATH, PARAMETERS_FILENAME);

    public static IParameters Parameters { get; private set; }

    protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject() {
      return new RibbonSMA();
    }

    private void ThisAddIn_Startup(object sender, System.EventArgs e) {
      Parameters = new TParametersRegistry();
      if (!Parameters.Init()) {
        return;
      }
      if (!Parameters.Read()) {
        return;
      }

      Logger.LogInfo($"SMMT v{APPLICATION_VERSION} started");

    }

    private void ThisAddIn_Shutdown(object sender, System.EventArgs e) {
      Logger.LogInfo($"SMMT v{APPLICATION_VERSION} exited gracefuly");
    }


    #region VSTO generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup() {
      this.Startup += new System.EventHandler(ThisAddIn_Startup);
      this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
    }

    #endregion
  }
}
