using Microsoft.Office.Core;

using SMA.Support;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMA {
  public partial class FormParams : Form {

    private readonly IParameters Parameters;
    public IParameters NewParameters { get; private set; }

    private ILogger Logger => Parameters?.Logger ?? ThisAddIn.DEFAULT_LOGGER;

    public FormParams() {
      InitializeComponent();
    }

    public FormParams(IParameters parameters) : this() {
      Parameters = parameters;
    }


    private void Form1_Load(object sender, EventArgs e) {
      if (Parameters == null) {
        MessageBox.Show("Paramètres non disponibles, fermeture de la fenêtre des paramètres.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        this.DialogResult = DialogResult.Cancel;
        this.Close();
      }
      txtRecipient.Text = Parameters.Recipient;
      txtPrefix.Text = Parameters.Prefix;
      txtCategory.Text = Parameters.Category;
      txtLogFilename.Text = Parameters.LogFilename;
      chkAskConfirmation.Checked = Parameters.WithConfirmation;
      chkCleanupSentMessages.Checked = Parameters.CleanupSentMessages;
    }

    private void btnResetDefault_Click(object sender, EventArgs e) {
      if (MessageBox.Show("Confirmer la réinitialisation des paramètres par défaut ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
        return;
      }
      txtRecipient.Text = AParameters.DEFAULT_RECIPIENT;
      txtPrefix.Text = AParameters.DEFAULT_PREFIX;
      txtCategory.Text = AParameters.DEFAULT_CATEGORY;
      txtLogFilename.Text = AParameters.DEFAULT_LOG_FULL_FILENAME;
      chkAskConfirmation.Checked = AParameters.DEFAULT_WITH_CONFIRMATION;
      chkCleanupSentMessages.Checked = AParameters.DEFAULT_CLEANUP_SENT_MESSAGES;
    }

    private void btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btnOk_Click(object sender, EventArgs e) {
      if (MessageBox.Show("Confirmer les nouveaux paramètres ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
        return;
      }
      NewParameters = new TParametersRegistry() {
        Recipient = txtRecipient.Text,
        Prefix = txtPrefix.Text,
        Category = txtCategory.Text,
        LogFilename = txtLogFilename.Text,
        WithConfirmation = chkAskConfirmation.Checked,
        CleanupSentMessages = chkCleanupSentMessages.Checked
      };
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

  }
}
