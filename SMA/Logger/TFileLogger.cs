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
      Filename = filename;
      string FilePath = Path.GetDirectoryName(filename);
      try {
        if (!Directory.Exists(FilePath)) {
          Directory.CreateDirectory(FilePath);
        }
      } catch (Exception ex) {
        // Silently fail — do NOT show MessageBox during initialization
        System.Diagnostics.Debug.WriteLine($"Could not create log directory {FilePath.WithQuotes()}: {ex.Message}");
      }
    }

    public override void Log(string message) {
      try {
        using (StreamWriter sw = new StreamWriter(Filename, true, Encoding.Default)) {
          sw.AutoFlush = true;
          string[] lines = message.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
          foreach (string line in lines) {
            sw.WriteLine($"{DateTime.Now:u}: {line}");
          }
        }
      } catch (Exception ex) {
        // Silently fail — do NOT show MessageBox
        System.Diagnostics.Debug.WriteLine($"Could not write to log file {Filename.WithQuotes()}: {ex.Message}");
      }
    }

    public override void Clear() {
      try {
        File.Delete(Filename);
        base.Clear();
      } catch {
        // Silently ignore deletion errors
      }
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