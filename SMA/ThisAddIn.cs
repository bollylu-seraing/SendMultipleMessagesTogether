using System;
using System.IO;
using System.Threading;

namespace SMA {
  public partial class ThisAddIn {

    public const string DEFAULT_APPLICATION_NAME = "SMA";
    public static Version APPLICATION_VERSION = new Version(1, 3, 9);

    public const string PARAMETERS_FILENAME = "ApplicationParameters.conf";
    public static string PARAMETERS_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DEFAULT_APPLICATION_NAME);

    public static ILogger DEFAULT_LOGGER => new TFileLogger(AParameters.DEFAULT_LOG_FULL_FILENAME);
    public static ILogger Logger => Parameters?.Logger ?? DEFAULT_LOGGER;

    public static string ApplicationPath { get; private set; } = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    public static string ApplicationParameters { get; private set; } = Path.Combine(PARAMETERS_PATH, PARAMETERS_FILENAME);

    public static IParameters Parameters { get; private set; }

    public static INotify Notifier { get; private set; } = new TNotifyMessagebox();

    protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject() {
      return new RibbonSMA();
    }

    private void ThisAddIn_Startup(object sender, System.EventArgs e) {
      try {
        // Initialize parameters with a timeout to prevent Outlook hang
        bool initialized = false;
        System.Threading.Thread initThread = new System.Threading.Thread(() => {
          try {
            Parameters = new TParametersRegistry();
            if (!Parameters.Init()) {
              return;
            }
            if (!Parameters.Read()) {
              return;
            }
            initialized = true;
          } catch (Exception ex) {
            DEFAULT_LOGGER.LogError("Fatal error during parameter initialization", ex);
          }
        });

        initThread.IsBackground = true;
        initThread.Start();

        // Wait max 5 seconds for initialization
        if (!initThread.Join(5000)) {
          DEFAULT_LOGGER.LogError("Parameter initialization timed out — using defaults");
          initThread.Abort();
          return;
        }

        if (initialized) {
          Logger.LogInfo($"SMA v{APPLICATION_VERSION} started");
        }
      } catch (Exception ex) {
        DEFAULT_LOGGER.LogError("Fatal error during startup", ex);
      }
    }

    private void ThisAddIn_Shutdown(object sender, System.EventArgs e) {
      Logger.LogInfo($"SMA v{APPLICATION_VERSION} exited gracefully");
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
