using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMultipleMessagesTogether {
  internal class TFileLogger : ALogger {
    readonly string Filename;
    public TFileLogger(string filename) {
      MessageBox.Show($"Log file: {filename}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
      Filename = filename;
      string FilePath = Path.GetDirectoryName(filename);
      try {
        if (!Directory.Exists(FilePath)) {
          Directory.CreateDirectory(FilePath);
        }
      } catch (Exception ex) {
        MessageBox.Show($"Could not create log directory {FilePath}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public override void Log(string message) {
      try {
        File.AppendAllText(Filename, $"{DateTime.Now:u}: {message}{Environment.NewLine}");
      } catch {
        MessageBox.Show($"Could not write to log file {Filename}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
