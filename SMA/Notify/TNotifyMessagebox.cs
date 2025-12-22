using System.Windows.Forms;

namespace SMA {

  public class TNotifyMessagebox : INotify {
    
    private bool isStartupPhase = true;

    public TNotifyMessagebox() {
      // Startup phase lasts only during initial add-in load
      // After 2 seconds, re-enable modal dialogs
      System.Threading.Timer startupTimer = null;
      startupTimer = new System.Threading.Timer(
        (state) => {
          isStartupPhase = false;
          startupTimer?.Dispose();
        },
        null,
        System.TimeSpan.FromSeconds(2),
        System.TimeSpan.FromMilliseconds(-1));
    }

    public void MessageInfo(string message) {
      if (isStartupPhase) {
        // Silent during startup — don't block Outlook
        System.Diagnostics.Debug.WriteLine($"[INFO] {message}");
        return;
      }
      MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void MessageError(string message) {
      if (isStartupPhase) {
        // Silent during startup — log only, don't block
        System.Diagnostics.Debug.WriteLine($"[ERROR] {message}");
        return;
      }
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public DialogResult MessageYesNo(string message) {
      if (isStartupPhase) {
        // Default to "No" during startup to avoid blocking
        return DialogResult.No;
      }
      return MessageBox.Show(message, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    public DialogResult MessageOkCancel(string message) {
      if (isStartupPhase) {
        // Default to "Cancel" during startup to avoid blocking
        return DialogResult.Cancel;
      }
      return MessageBox.Show(message, "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
    }

  }

}