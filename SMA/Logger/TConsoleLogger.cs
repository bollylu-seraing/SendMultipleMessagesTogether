using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMA {
  internal class TConsoleLogger : ALogger {

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool AllocConsole();

    public TConsoleLogger() : base() {

    }

    public override void Log(string message) {
      Console.WriteLine($"{DateTime.Now:u}: {message}");
    }

    public override void Clear() {
      Console.Clear();
      base.Clear();
    }

    public override string GetLogContent() {
      return ""; // Not implemented
    }
  }
}
