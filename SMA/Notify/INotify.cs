using System.Windows.Forms;

namespace SMA {

  public interface INotify {
    void MessageInfo(string message);
    void MessageError(string message);

    DialogResult MessageYesNo(string message);
    DialogResult MessageOkCancel(string message);
  }

}