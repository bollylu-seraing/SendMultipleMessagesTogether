using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMultipleMessagesTogether {
  internal abstract class ALogger : ILogger {

    public abstract void Log(string message);

    public void LogError(string message, Exception ex) {
      Log($"{message}: {ex.Message}");
    }


  }
}
