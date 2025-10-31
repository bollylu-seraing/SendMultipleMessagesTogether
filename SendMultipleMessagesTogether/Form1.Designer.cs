using System.Windows.Forms;

namespace SendMultipleMessagesTogether {
  partial class Form1 {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    Label lblRecipient = new Label() { Text = "Récipient :" };
    Label lblPrefix = new Label() { Text = "Préfixe :" };
    Label lblCategory = new Label() { Text = "Catégorie :" };
    Label lblLogFilename = new Label() { Text = "Fichier log :" };
    Label lblAskConfirmation = new Label() { Text = "Confirmation pour indication" };
    Label lblCleanupSentMessage = new Label() { Text = "Nettoyer les messages envoyés" };

    TextBox txtRecipient = new TextBox();
    TextBox txtPrefix = new TextBox();
    TextBox txtCategory = new TextBox();
    TextBox txtLogFilename = new TextBox();
    CheckBox chkAskConfirmation = new CheckBox();
    CheckBox chkCleanupSentMessages = new CheckBox();

    Button btnOk = new Button();
    Button btnCancel = new Button();
    Button btnResetDefault = new Button();

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.SuspendLayout();
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Paramètres Indicateur IA.Docs";
      this.Load += new System.EventHandler(this.Form1_Load);

      this.lblRecipient.Top = 20;
      this.lblPrefix.Top = lblRecipient.Bottom + 20;
      this.lblCategory.Top = lblPrefix.Bottom + 20;
      this.lblLogFilename.Top = lblCategory.Bottom + 20;
      this.lblAskConfirmation.Width = 200;
      this.lblAskConfirmation.Top = lblLogFilename.Bottom + 20;
      this.lblCleanupSentMessage.Width = 200;
      this.lblCleanupSentMessage.Top = lblAskConfirmation.Bottom + 20;

      this.lblRecipient.Left = 10;
      this.lblPrefix.Left = 10;
      this.lblCategory.Left = 10;
      this.lblLogFilename.Left = 10;
      this.lblAskConfirmation.Left = 10;
      this.lblCleanupSentMessage.Left = 10;

      this.txtRecipient.Left = lblRecipient.Right + 10;
      this.txtRecipient.Top = lblRecipient.Top;
      this.txtRecipient.Width = 200;
      this.txtPrefix.Left = lblPrefix.Right + 10;
      this.txtPrefix.Top = lblPrefix.Top;
      this.txtPrefix.Width = 200;
      this.txtCategory.Left = lblCategory.Right + 10;
      this.txtCategory.Top = lblCategory.Top;
      this.txtCategory.Width = 200;
      this.txtLogFilename.Left = lblLogFilename.Right + 10;
      this.txtLogFilename.Top = lblLogFilename.Top;
      this.txtLogFilename.Width = 500;
      this.chkAskConfirmation.Left = lblAskConfirmation.Right + 10;
      this.chkAskConfirmation.Top = lblAskConfirmation.Top;
      this.chkCleanupSentMessages.Left = lblCleanupSentMessage.Right + 10;
      this.chkCleanupSentMessages.Top = lblCleanupSentMessage.Top;

      Control LastControl = lblCleanupSentMessage;

      btnOk.Text = "OK";
      btnOk.Left = 10;
      btnOk.Top = LastControl.Bottom + 20;

      btnCancel.Text = "Annuler";
      btnCancel.Left = btnOk.Right + 10;
      btnCancel.Top = LastControl.Bottom + 20;

      btnResetDefault.Text = "Réinitialiser";
      btnResetDefault.Left = btnCancel.Right + 10;
      btnResetDefault.Top = LastControl.Bottom + 20;
      btnResetDefault.BackColor = System.Drawing.Color.LightCoral;
      btnResetDefault.ForeColor = System.Drawing.Color.White;



      btnOk.Click += (s, ev) => {
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
      };

      btnResetDefault.Click += (s, ev) => {
        if (MessageBox.Show("Confirmer la réinitialisation des paramètres par défaut ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
          return;
        }
        txtRecipient.Text = AParameters.DEFAULT_RECIPIENT;
        txtPrefix.Text = AParameters.DEFAULT_PREFIX;
        txtCategory.Text = AParameters.DEFAULT_CATEGORY;
        txtLogFilename.Text = AParameters.DEFAULT_LOG_FULL_FILENAME;
        chkAskConfirmation.Checked = AParameters.DEFAULT_WITH_CONFIRMATION;
        chkCleanupSentMessages.Checked = AParameters.DEFAULT_CLEANUP_SENT_MESSAGES;
      };

      btnCancel.Click += (s, ev) => {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
      };
      
      this.Controls.Add(lblRecipient);
      this.Controls.Add(lblPrefix);
      this.Controls.Add(lblCategory);
      this.Controls.Add(lblLogFilename);
      this.Controls.Add(lblAskConfirmation);
      this.Controls.Add(lblCleanupSentMessage);

      this.Controls.Add(btnOk);
      this.Controls.Add(btnCancel);
      this.Controls.Add(btnResetDefault);
      
      this.Controls.Add(txtRecipient);
      this.Controls.Add(txtPrefix);
      this.Controls.Add(txtCategory);
      this.Controls.Add(txtLogFilename);
      this.Controls.Add(chkAskConfirmation);
      this.Controls.Add(chkCleanupSentMessages);

      this.Width = txtLogFilename.Right + 40;
      this.Height = btnOk.Bottom + 60;

      this.ResumeLayout(false);

    }

    #endregion
  }
}