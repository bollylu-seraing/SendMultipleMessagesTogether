using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMultipleMessagesTogether {
  internal abstract class ALogger : ILogger {

    public abstract void Log(string message);

    public virtual void LogInfo(string message) {
      Log($"Info  : {message}");
    }

    public virtual void LogWarning(string message) {
      Log($"Warn  : {message}");
    }
    public virtual void LogError(string message, Exception ex) {
      Log($"Error : {message} : {ex.Message}");
    }

    public virtual void LogError(string message) {
      Log($"Error : {message}");
    }


  }
}
