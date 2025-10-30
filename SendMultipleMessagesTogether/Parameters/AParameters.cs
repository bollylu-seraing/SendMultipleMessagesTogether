using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMultipleMessagesTogether {
  public abstract class AParameters : IParameters {

    protected const string KEY_PREFIX = "Prefix";
    public const string DEFAULT_PREFIX = "Indicateur - TR: ";

    protected const string KEY_RECIPIENT = "Recipient";
    public const string DEFAULT_RECIPIENT = "seraing-docs.imio-app.be";
    //public const string DEFAULT_RECIPIENT = "l.bolly@seraing.be";

    protected const string KEY_LOG_TYPE = "LogType";
    //protected static string DEFAULT_LOG_TYPE = ELogType.MessageBox.ToString();
    public static string DEFAULT_LOG_TYPE = ELogType.File.ToString();

    protected const string KEY_CATEGORY = "Category";
    public const string DEFAULT_CATEGORY = "indicaté";

    protected const string KEY_LOG_FILENAME = "LogFilename";
    protected static string DEFAULT_LOG_FILENAME = "SendMultipleMessagesTogether.log";
    public static string DEFAULT_LOG_FULL_FILENAME = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ThisAddIn.DEFAULT_APPLICATION_NAME, DEFAULT_LOG_FILENAME);

    protected const string KEY_WITH_CONFIRMATION = "WithConfirmation";
    public const bool DEFAULT_WITH_CONFIRMATION = false;

    private ILogger _logger = null;
    public ILogger Logger {
      get {
        if (_logger == null) {
          _logger = new TMessageBoxLogger();
        }
        return _logger;
      }
      protected set {
        _logger = value;
      }
    }

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    protected AParameters() { }

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

    public abstract bool Init();
    public abstract bool Read();
    public abstract bool Save();

    public string LogTypeString { get; set; } = DEFAULT_LOG_TYPE;
    public ELogType LogType => (ELogType)Enum.Parse(typeof(ELogType), LogTypeString ?? DEFAULT_LOG_TYPE);
    public string LogFilename { get; set; } = DEFAULT_LOG_FULL_FILENAME;
    public string Recipient { get; set; } = DEFAULT_RECIPIENT;
    public string Prefix { get; set; } = DEFAULT_PREFIX;
    public string Category { get; set; } = DEFAULT_CATEGORY;
    public bool WithConfirmation { get; set; } = DEFAULT_WITH_CONFIRMATION;
  }
}
