using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMultipleMessagesTogether {
  public interface ILogger {
    void Log(string message);
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message);
    void LogError(string message, Exception ex);
  }

}
