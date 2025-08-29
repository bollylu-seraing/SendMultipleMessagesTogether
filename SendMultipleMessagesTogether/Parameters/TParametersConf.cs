using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SendMultipleMessagesTogether.Support;

namespace SendMultipleMessagesTogether {
  public class TParametersConf : AParameters {

    private const string SEPARATOR = "=";
    private const string COMMENT = "#";

    public string ConfigFile { get; set; } = string.Empty;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TParametersConf(ILogger logger, string configFile) : base(logger) {
      ConfigFile = configFile ?? throw new ArgumentNullException(nameof(configFile));
    }
    public TParametersConf(IParameters parameters) : base(parameters) {
      if (parameters is TParametersConf conf) {
        ConfigFile = conf.ConfigFile;
      }
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override bool Read() {
      if (!File.Exists(ConfigFile)) {
        Logger.LogError($"Configuration file {ConfigFile.WithQuotes()} does not exist.");
        return false;
      }

      string TextLine;
      try {
        using (StreamReader reader = new StreamReader(ConfigFile)) {
          TextLine = reader.ReadLine();
          while (TextLine != null) {
            if (TextLine.TrimStart().StartsWith(COMMENT)) {
              continue;
            }
            string ParameterName = TextLine.Before(SEPARATOR).Trim();
            string ParameterValue = TextLine.After(SEPARATOR);
            switch (ParameterName.ToLowerInvariant()) {
              case KEY_RECIPIENT:
                Recipient = ParameterValue.Trim().ToLowerInvariant();
                break;
              case KEY_PREFIX:
                Prefix = ParameterValue;
                break;
              case KEY_LOG_TYPE:
                LogTypeString = ParameterValue.Trim();
                break;
              case KEY_LOG_FILENAME:
                LogFilename = ParameterValue.RemoveExternalQuotes();
                break;
              default:
                Logger.LogWarning($"Unknown parameter {ParameterName.WithQuotes()} in configuration file {ConfigFile.WithQuotes()}");
                break;
            }
          }
        }
        if (LogType == ELogType.File) {
          string LogFilePath = Path.GetDirectoryName(LogFilename);
          if (!Directory.Exists(LogFilePath)) {
            Directory.CreateDirectory(LogFilePath);
          }
          Logger = new TFileLogger(LogFilename);
        }
        return true;
      } catch (Exception ex) {
        Logger.LogError($"Error reading parameters from configuration file {ConfigFile.WithQuotes()}", ex);
        return false;
      }
    }

    public override bool Save() {
      if (ConfigFile == "") {
        Logger.LogError("Configuration file name is empty.");
        return false;
      }

      try {
        using (StreamWriter writer = new StreamWriter(ConfigFile, false)) {
          writer.WriteLine("# Configuration file for SendMultipleMessagesTogether");
          writer.WriteLine($"{KEY_RECIPIENT}{SEPARATOR}{Recipient}");
          writer.WriteLine($"{KEY_PREFIX}{SEPARATOR}{Prefix}");
          writer.WriteLine($"{KEY_LOG_TYPE}{SEPARATOR}{LogTypeString}");
          writer.WriteLine($"{KEY_LOG_FILENAME}{SEPARATOR}{LogFilename.Trim().WithQuotes()}");
        }
        return true;
      } catch (Exception ex) {
        Logger.LogError($"Error saving parameters to configuration file {ConfigFile.WithQuotes()}", ex);
        return false;
      }
    }
  }
}
