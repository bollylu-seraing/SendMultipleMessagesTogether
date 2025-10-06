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

    public override bool Init() {
      if (ConfigFile == "") {
        Logger.LogError("Configuration file name is empty.");
        return false;
      }

      try {
        using (StreamWriter writer = new StreamWriter(ConfigFile, false)) {
          writer.WriteLine("# Configuration file for SendMultipleMessagesTogether");
          writer.WriteLine($"{KEY_RECIPIENT}{SEPARATOR}{DEFAULT_RECIPIENT.WithQuotes()}");
          writer.WriteLine($"{KEY_PREFIX}{SEPARATOR}{DEFAULT_PREFIX.WithQuotes()}");
          writer.WriteLine($"{KEY_LOG_TYPE}{SEPARATOR}{DEFAULT_LOG_TYPE}");
          writer.WriteLine($"{KEY_LOG_FILENAME}{SEPARATOR}{DEFAULT_LOG_FULL_FILENAME.WithQuotes()}");
          writer.WriteLine($"{KEY_CATEGORY}{SEPARATOR}{DEFAULT_CATEGORY.WithQuotes()}");
        }
        return true;
      } catch (Exception ex) {
        Logger.LogError($"Error saving parameters to configuration file {ConfigFile.WithQuotes()}", ex);
        return false;
      }
    }
    
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
            string ParameterValue = TextLine.After(SEPARATOR).RemoveExternalQuotes();
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
              case KEY_CATEGORY:
                Category = ParameterValue;
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
          writer.WriteLine($"{KEY_RECIPIENT}{SEPARATOR}{Recipient.WithQuotes()}");
          writer.WriteLine($"{KEY_PREFIX}{SEPARATOR}{Prefix.WithQuotes()}");
          writer.WriteLine($"{KEY_LOG_TYPE}{SEPARATOR}{LogTypeString.WithQuotes()}");
          writer.WriteLine($"{KEY_LOG_FILENAME}{SEPARATOR}{LogFilename.Trim().WithQuotes()}");
          writer.WriteLine($"{KEY_CATEGORY}{SEPARATOR}{Category.WithQuotes()}");
        }
        return true;
      } catch (Exception ex) {
        Logger.LogError($"Error saving parameters to configuration file {ConfigFile.WithQuotes()}", ex);
        return false;
      }
    }
  }
}
