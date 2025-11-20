using SMA.Support;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMA {
  internal class TFileLogger : ALogger {
    readonly string Filename;

    public TFileLogger(string filename) {
      //MessageBox.Show($"Log file: {filename.WithQuotes()}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
      Filename = filename;
      string FilePath = Path.GetDirectoryName(filename);
      try {
        if (!Directory.Exists(FilePath)) {
          Directory.CreateDirectory(FilePath);
        }
      } catch (Exception ex) {
        MessageBox.Show($"Could not create log directory {FilePath.WithQuotes()}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public override void Log(string message) {
      try {
        using (StreamWriter sw = new StreamWriter(Filename, true, Encoding.Default)) {
          sw.AutoFlush = true;
          foreach (string line in message.Split(new[] { Environment.NewLine }, StringSplitOptions.None)) {
            sw.WriteLine($"{DateTime.Now:u}: {line}");
          }
        }
      } catch {
        MessageBox.Show($"Could not write to log file {Filename.WithQuotes()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public override void Clear() {
      File.Delete(Filename);
      base.Clear();
    }

    public override string GetLogContent() {
      try {
        return File.ReadAllText(Filename, Encoding.Default);
      } catch (Exception ex) {
        return ex.Message;
      }
    }
  }
}