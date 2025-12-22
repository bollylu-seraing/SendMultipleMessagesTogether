using System;
using System.Drawing;
using System.Windows.Forms;

namespace SMA {

  public class TNotifyForm : INotify {

    private NotificationForm notificationForm;

    public void MessageInfo(string message) {
      ShowNotificationForm(message, "Information", MessageBoxIcon.Information, null);
    }

    public void MessageError(string message) {
      ShowNotificationForm(message, "Error", MessageBoxIcon.Error, null);
    }

    public DialogResult MessageYesNo(string message) {
      return ShowNotificationFormBlocking(message, "Question", MessageBoxIcon.Question, DialogResult.No, DialogResult.Yes);
    }

    public DialogResult MessageOkCancel(string message) {
      return ShowNotificationFormBlocking(message, "Confirmation", MessageBoxIcon.Question, DialogResult.Cancel, DialogResult.OK);
    }

    private void ShowNotificationForm(string message, string title, MessageBoxIcon icon, DialogResult? expectedResult) {
      if (notificationForm != null && !notificationForm.IsDisposed) {
        notificationForm.Close();
      }

      notificationForm = new NotificationForm(message, title, icon, expectedResult);
      notificationForm.Show();
    }

    private DialogResult ShowNotificationFormBlocking(string message, string title, MessageBoxIcon icon, DialogResult negativeButton, DialogResult positiveButton) {
      using (NotificationForm form = new NotificationForm(message, title, icon, null, negativeButton, positiveButton)) {
        return form.ShowDialog();
      }
    }

  }

  /// <summary>
  /// Non-blocking notification form that displays messages with optional buttons
  /// </summary>
  public partial class NotificationForm : Form {

    private MessageBoxIcon messageIcon;
    private DialogResult? singleButtonResult;
    private DialogResult negativeButtonResult;
    private DialogResult positiveButtonResult;

    public NotificationForm(
      string message,
      string title,
      MessageBoxIcon icon,
      DialogResult? singleButtonResult = null,
      DialogResult negativeButtonResult = DialogResult.No,
      DialogResult positiveButtonResult = DialogResult.Yes) {

      this.messageIcon = icon;
      this.singleButtonResult = singleButtonResult;
      this.negativeButtonResult = negativeButtonResult;
      this.positiveButtonResult = positiveButtonResult;

      InitializeComponent();
      this.Text = title;
      this.labelMessage.Text = message;
      this.StartPosition = FormStartPosition.CenterParent;
      this.TopMost = true;

      ConfigureButtons(singleButtonResult != null);
      SetIconImage(icon);
    }

    private void InitializeComponent() {
      this.labelMessage = new Label();
      this.panelIcon = new Panel();
      this.pictureBoxIcon = new PictureBox();
      this.panelButtons = new Panel();
      this.buttonOK = new Button();
      this.buttonCancel = new Button();
      this.buttonYes = new Button();
      this.buttonNo = new Button();

      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
      this.SuspendLayout();

      // labelMessage
      this.labelMessage.AutoSize = false;
      this.labelMessage.Dock = DockStyle.Fill;
      this.labelMessage.Location = new System.Drawing.Point(60, 10);
      this.labelMessage.Margin = new Padding(3, 10, 10, 10);
      this.labelMessage.Name = "labelMessage";
      this.labelMessage.Size = new System.Drawing.Size(330, 80);
      this.labelMessage.TabIndex = 0;
      this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

      // panelIcon
      this.panelIcon.Location = new System.Drawing.Point(10, 10);
      this.panelIcon.Name = "panelIcon";
      this.panelIcon.Size = new System.Drawing.Size(40, 40);
      this.panelIcon.TabIndex = 1;

      // pictureBoxIcon
      this.pictureBoxIcon.Dock = DockStyle.Fill;
      this.pictureBoxIcon.Location = new System.Drawing.Point(0, 0);
      this.pictureBoxIcon.Name = "pictureBoxIcon";
      this.pictureBoxIcon.Size = new System.Drawing.Size(40, 40);
      this.pictureBoxIcon.SizeMode = PictureBoxSizeMode.Zoom;
      this.pictureBoxIcon.TabIndex = 0;

      // panelButtons
      this.panelButtons.Dock = DockStyle.Bottom;
      this.panelButtons.Height = 50;
      this.panelButtons.Location = new System.Drawing.Point(0, 110);
      this.panelButtons.Name = "panelButtons";
      this.panelButtons.Padding = new Padding(10);
      this.panelButtons.TabIndex = 2;

      // buttonOK
      this.buttonOK.Location = new System.Drawing.Point(240, 15);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 0;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
      this.panelButtons.Controls.Add(this.buttonOK);

      // buttonCancel
      this.buttonCancel.Location = new System.Drawing.Point(165, 15);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 1;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
      this.panelButtons.Controls.Add(this.buttonCancel);

      // buttonYes
      this.buttonYes.Location = new System.Drawing.Point(240, 15);
      this.buttonYes.Name = "buttonYes";
      this.buttonYes.Size = new System.Drawing.Size(75, 23);
      this.buttonYes.TabIndex = 2;
      this.buttonYes.Text = "Yes";
      this.buttonYes.UseVisualStyleBackColor = true;
      this.buttonYes.Click += (s, e) => { this.DialogResult = positiveButtonResult; this.Close(); };
      this.panelButtons.Controls.Add(this.buttonYes);

      // buttonNo
      this.buttonNo.Location = new System.Drawing.Point(165, 15);
      this.buttonNo.Name = "buttonNo";
      this.buttonNo.Size = new System.Drawing.Size(75, 23);
      this.buttonNo.TabIndex = 3;
      this.buttonNo.Text = "No";
      this.buttonNo.UseVisualStyleBackColor = true;
      this.buttonNo.Click += (s, e) => { this.DialogResult = negativeButtonResult; this.Close(); };
      this.panelButtons.Controls.Add(this.buttonNo);

      // NotificationForm
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(400, 160);
      this.ControlBox = false;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "NotificationForm";
      this.ShowInTaskbar = false;
      this.Controls.Add(this.panelIcon);
      this.Controls.Add(this.labelMessage);
      this.Controls.Add(this.panelButtons);

      panelIcon.Controls.Add(this.pictureBoxIcon);

      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
      this.ResumeLayout(false);
    }

    private void ConfigureButtons(bool singleButtonMode) {
      this.buttonOK.Visible = singleButtonMode;
      this.buttonCancel.Visible = !singleButtonMode;
      this.buttonYes.Visible = !singleButtonMode;
      this.buttonNo.Visible = !singleButtonMode;
    }

    private void SetIconImage(MessageBoxIcon icon) {
      try {
        switch (icon) {
          case MessageBoxIcon.Information:
            this.pictureBoxIcon.Image = SystemIcons.Information.ToBitmap();
            break;
          case MessageBoxIcon.Warning:
            this.pictureBoxIcon.Image = SystemIcons.Warning.ToBitmap();
            break;
          case MessageBoxIcon.Error:
            this.pictureBoxIcon.Image = SystemIcons.Error.ToBitmap();
            break;
          case MessageBoxIcon.Question:
            this.pictureBoxIcon.Image = SystemIcons.Question.ToBitmap();
            break;
          default:
            this.pictureBoxIcon.Image = null;
            break;
        }
      } catch {
        this.pictureBoxIcon.Image = null;
      }
    }

    private Label labelMessage;
    private Panel panelIcon;
    private PictureBox pictureBoxIcon;
    private Panel panelButtons;
    private Button buttonOK;
    private Button buttonCancel;
    private Button buttonYes;
    private Button buttonNo;

  }

}