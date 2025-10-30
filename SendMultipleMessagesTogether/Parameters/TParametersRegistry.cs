﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

using SendMultipleMessagesTogether.Support;

namespace SendMultipleMessagesTogether {
  public class TParametersRegistry : AParameters {

    private static readonly string KeyName = $@"Software\{ThisAddIn.DEFAULT_APPLICATION_NAME}";

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TParametersRegistry() {
    }
    public TParametersRegistry(ILogger logger) {
      Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public TParametersRegistry(IParameters parameters) : base(parameters) {
      Recipient = parameters.Recipient;
      Prefix = parameters.Prefix;
      LogTypeString = parameters.LogTypeString;
      LogFilename = parameters.LogFilename;
      Category = parameters.Category;
      WithConfirmation = parameters.WithConfirmation;

    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override bool Init() {
      try {
        RegistryKey HKCU = Registry.CurrentUser;
        using (RegistryKey ApplicationKey = HKCU.OpenSubKey(KeyName, true)) {
          if (ApplicationKey == null) {
            using (RegistryKey NewApplicationKey = HKCU.CreateSubKey(KeyName)) {
              NewApplicationKey.SetValue(KEY_RECIPIENT, Recipient);
              NewApplicationKey.SetValue(KEY_PREFIX, Prefix);
              NewApplicationKey.SetValue(KEY_LOG_TYPE, LogTypeString);
              NewApplicationKey.SetValue(KEY_LOG_FILENAME, LogFilename);
              NewApplicationKey.SetValue(KEY_CATEGORY, Category);
              NewApplicationKey.SetValue(KEY_WITH_CONFIRMATION, WithConfirmation);
            }
          }
        }
        return true;
      } catch (Exception ex) {
        Logger?.LogError($"Error initializing parameters in registry key {$"HKCU\\{KeyName}".WithQuotes()}", ex);
        return false;
      }
    }

    public override bool Read() {
      try {
        RegistryKey HKCU = Registry.CurrentUser;
        using (RegistryKey ApplicationKey = Registry.CurrentUser.OpenSubKey(KeyName, false)) {
          if (!Init()) {
            return false;
          }
          Recipient = (string)(ApplicationKey?.GetValue(KEY_RECIPIENT) ?? DEFAULT_RECIPIENT);
          Prefix = (string)(ApplicationKey?.GetValue(KEY_PREFIX) ?? DEFAULT_PREFIX);
          LogTypeString = (string)(ApplicationKey?.GetValue(KEY_LOG_TYPE) ?? DEFAULT_LOG_TYPE);
          LogFilename = ((string)(ApplicationKey?.GetValue(KEY_LOG_FILENAME) ?? DEFAULT_LOG_FULL_FILENAME)).RemoveExternalQuotes();
          Category = (string)(ApplicationKey?.GetValue(KEY_CATEGORY) ?? DEFAULT_CATEGORY);
          WithConfirmation = ((string)(ApplicationKey?.GetValue(KEY_WITH_CONFIRMATION) ?? DEFAULT_WITH_CONFIRMATION)).ToBool();
        }
        if (LogType == ELogType.File) {
          string LogFilePath = Path.GetDirectoryName(LogFilename);
          if (!Directory.Exists(LogFilePath)) {
            Directory.CreateDirectory(LogFilePath);
          }
          Logger = new TFileLogger(LogFilename);
        } else {
          Logger = new TMessageBoxLogger();
        }
        return true;
      } catch (Exception ex) {
        Logger.LogError($"Error reading parameters from registry key {$"HKCU\\{KeyName}".WithQuotes()}", ex);
        Logger.LogInfo("Attempt to save the registry keys with default values.");
        return Save();
      }
    }

    public override bool Save() {

      try {
        RegistryKey HKCU = Registry.CurrentUser;
        using (RegistryKey ApplicationKey = HKCU.OpenSubKey(KeyName, true)) {
          if (ApplicationKey == null) {
            using (RegistryKey NewApplicationKey = HKCU.CreateSubKey(KeyName)) {
              NewApplicationKey.SetValue(KEY_RECIPIENT, Recipient);
              NewApplicationKey.SetValue(KEY_PREFIX, Prefix);
              NewApplicationKey.SetValue(KEY_LOG_TYPE, LogTypeString);
              NewApplicationKey.SetValue(KEY_LOG_FILENAME, LogFilename);
              NewApplicationKey.SetValue(KEY_CATEGORY, Category);
              NewApplicationKey.SetValue(KEY_WITH_CONFIRMATION, WithConfirmation);
            }
          } else {
            ApplicationKey.SetValue(KEY_RECIPIENT, Recipient);
            ApplicationKey.SetValue(KEY_PREFIX, Prefix);
            ApplicationKey.SetValue(KEY_LOG_TYPE, LogTypeString);
            ApplicationKey.SetValue(KEY_LOG_FILENAME, LogFilename);
            ApplicationKey.SetValue(KEY_CATEGORY, Category);
            ApplicationKey.SetValue(KEY_WITH_CONFIRMATION, WithConfirmation);
          }
        }
        return true;
      } catch (Exception ex) {
        Logger?.LogError($"Error writing parameters to registry key {$"HKCU\\{KeyName}".WithQuotes()}", ex);
        return false;
      }
    }
  }
}
