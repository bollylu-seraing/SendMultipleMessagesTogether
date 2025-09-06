using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMultipleMessagesTogether {
  public abstract class AParameters : IParameters {

    protected const string KEY_PREFIX = "Prefix";
    protected const string DEFAULT_PREFIX = "SMMA - ";

    protected const string KEY_RECIPIENT = "Recipient";
    protected const string DEFAULT_RECIPIENT = "indic@missing.local";

    protected const string KEY_LOG_TYPE = "LogType";
    //protected static string DEFAULT_LOG_TYPE = ELogType.MessageBox.ToString();
    protected static string DEFAULT_LOG_TYPE = ELogType.File.ToString();

    protected const string KEY_LOG_FILENAME = "LogFilename";
    protected static string DEFAULT_LOG_FILENAME = "SendMultipleMessagesTogether.log";
    protected static string DEFAULT_LOG_FULL_FILENAME = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ThisAddIn.DEFAULT_APPLICATION_NAME, DEFAULT_LOG_FILENAME);

    public ILogger Logger { get; protected set; }

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    protected AParameters() {
      Logger = new TMessageBoxLogger();
    }

    protected AParameters(IParameters parameters) {
      if (parameters == null) {
        throw new ArgumentNullException(nameof(parameters));
      }
      Logger = parameters.Logger ?? throw new ArgumentNullException(nameof(parameters.Logger));
      LogFilename = parameters.LogFilename;
      Recipient = parameters.Recipient;
      Prefix = parameters.Prefix;
    }

    protected AParameters(ILogger logger) {
      Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public abstract bool Read();
    public abstract bool Save();

    protected string LogTypeString { get; set; } = DEFAULT_LOG_TYPE;
    public ELogType LogType => (ELogType)Enum.Parse(typeof(ELogType), LogTypeString ?? DEFAULT_LOG_TYPE);
    public string LogFilename { get; protected set; } = DEFAULT_LOG_FULL_FILENAME;
    public string Recipient { get; protected set; } = DEFAULT_RECIPIENT;
    public string Prefix { get; protected set; } = DEFAULT_PREFIX;

  }
}
