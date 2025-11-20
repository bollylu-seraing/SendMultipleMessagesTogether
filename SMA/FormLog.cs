using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMA {
  public partial class FormLog : Form {

    private readonly IParameters Parameters;
    private ILogger Logger => Parameters.Logger;

    public FormLog() {
      InitializeComponent();
    }

    public FormLog(IParameters parameters) : this() {
      Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
    }

    private void FormLog_Load(object sender, EventArgs e) {
      btnRefresh_Click(this, EventArgs.Empty);
    }

    private void btnRefresh_Click(object sender, EventArgs e) {
      txtLog.Clear();
      string[] Lines = Logger.GetLogContent().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
      foreach (string LineItem in Lines.Where(x => !string.IsNullOrWhiteSpace(x))) {

        int Position = txtLog.Text.Length;
        txtLog.AppendText(LineItem + Environment.NewLine);
        txtLog.Select(Position, LineItem.Length);

        if (LineItem.Contains(": Error :")) {
          txtLog.SelectionColor = Color.Red;
        } else if (LineItem.Contains(": Warn  :")) {
          txtLog.SelectionColor = Color.Coral;
        } else {
          txtLog.SelectionColor = txtLog.ForeColor;
        }
      }
      txtLog.SelectionStart = txtLog.Text.Length;
      txtLog.SelectionLength = 0;
      txtLog.SelectionColor = txtLog.ForeColor;
    }

    private void btnOk_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void btnClear_Click(object sender, EventArgs e) {
      if (MessageBox.Show("Effacer le contenu du fichier log ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
        return;
      }
      Logger.Clear();
      btnRefresh_Click(this, EventArgs.Empty);
    }
  }
}
