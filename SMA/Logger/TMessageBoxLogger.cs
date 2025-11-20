using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMA {
  internal class TMessageBoxLogger : ALogger {
    public override void Log(string message) {
      MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public override void LogInfo(string message) {
      MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public override void LogWarning(string message) {
      MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public override void LogError(string message, Exception ex) {
      MessageBox.Show($"{message} : {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public override void LogError(string message) {
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public override void Clear() {
      base.Clear();
    }

    public override string GetLogContent() {
      return ""; // Not implemented
    }
  }
}
