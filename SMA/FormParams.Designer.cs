using System;
using System.Windows.Forms;

namespace SMA {
  partial class FormParams {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
      this.chkAskConfirmation = new System.Windows.Forms.CheckBox();
      this.chkCleanupSentMessages = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.btnOk = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnResetDefault = new System.Windows.Forms.Button();
      this.txtRecipient = new System.Windows.Forms.TextBox();
      this.txtPrefix = new System.Windows.Forms.TextBox();
      this.txtCategory = new System.Windows.Forms.TextBox();
      this.txtLogFilename = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // chkAskConfirmation
      // 
      this.chkAskConfirmation.AutoSize = true;
      this.chkAskConfirmation.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkAskConfirmation.Location = new System.Drawing.Point(12, 143);
      this.chkAskConfirmation.Name = "chkAskConfirmation";
      this.chkAskConfirmation.Size = new System.Drawing.Size(194, 21);
      this.chkAskConfirmation.TabIndex = 1;
      this.chkAskConfirmation.Text = "Confirmation pour indication";
      this.chkAskConfirmation.UseVisualStyleBackColor = true;
      // 
      // chkCleanupSentMessages
      // 
      this.chkCleanupSentMessages.AutoSize = true;
      this.chkCleanupSentMessages.Checked = true;
      this.chkCleanupSentMessages.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkCleanupSentMessages.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkCleanupSentMessages.Location = new System.Drawing.Point(12, 170);
      this.chkCleanupSentMessages.Name = "chkCleanupSentMessages";
      this.chkCleanupSentMessages.Size = new System.Drawing.Size(480, 21);
      this.chkCleanupSentMessages.TabIndex = 1;
      this.chkCleanupSentMessages.Text = "Nettoyer automatiquement les messages indicatés dans les éléments envoyés";
      this.chkCleanupSentMessages.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(12, 19);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(73, 17);
      this.label1.TabIndex = 2;
      this.label1.Text = "Destination";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(12, 53);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(47, 17);
      this.label2.TabIndex = 2;
      this.label2.Text = "Préfixe";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(12, 84);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(65, 17);
      this.label3.TabIndex = 2;
      this.label3.Text = "Catégorie";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(12, 115);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(68, 17);
      this.label4.TabIndex = 2;
      this.label4.Text = "Fichier log";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // btnOk
      // 
      this.btnOk.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnOk.Location = new System.Drawing.Point(10, 229);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(75, 23);
      this.btnOk.TabIndex = 3;
      this.btnOk.Text = "Sauver";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCancel.Location = new System.Drawing.Point(91, 229);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Annuler";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnResetDefault
      // 
      this.btnResetDefault.AutoSize = true;
      this.btnResetDefault.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
      this.btnResetDefault.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnResetDefault.Location = new System.Drawing.Point(377, 227);
      this.btnResetDefault.Name = "btnResetDefault";
      this.btnResetDefault.Size = new System.Drawing.Size(125, 27);
      this.btnResetDefault.TabIndex = 3;
      this.btnResetDefault.Text = "Valeurs par défaut";
      this.btnResetDefault.UseVisualStyleBackColor = false;
      this.btnResetDefault.Click += new System.EventHandler(this.btnResetDefault_Click);
      // 
      // txtRecipient
      // 
      this.txtRecipient.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtRecipient.Location = new System.Drawing.Point(106, 19);
      this.txtRecipient.Name = "txtRecipient";
      this.txtRecipient.Size = new System.Drawing.Size(396, 25);
      this.txtRecipient.TabIndex = 4;
      // 
      // txtPrefix
      // 
      this.txtPrefix.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtPrefix.Location = new System.Drawing.Point(106, 50);
      this.txtPrefix.Name = "txtPrefix";
      this.txtPrefix.Size = new System.Drawing.Size(396, 25);
      this.txtPrefix.TabIndex = 4;
      // 
      // txtCategory
      // 
      this.txtCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtCategory.Location = new System.Drawing.Point(106, 81);
      this.txtCategory.Name = "txtCategory";
      this.txtCategory.Size = new System.Drawing.Size(396, 25);
      this.txtCategory.TabIndex = 4;
      // 
      // txtLogFilename
      // 
      this.txtLogFilename.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtLogFilename.Location = new System.Drawing.Point(106, 112);
      this.txtLogFilename.Name = "txtLogFilename";
      this.txtLogFilename.Size = new System.Drawing.Size(396, 25);
      this.txtLogFilename.TabIndex = 4;
      // 
      // FormParams
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.ClientSize = new System.Drawing.Size(514, 265);
      this.Controls.Add(this.txtLogFilename);
      this.Controls.Add(this.txtCategory);
      this.Controls.Add(this.txtPrefix);
      this.Controls.Add(this.txtRecipient);
      this.Controls.Add(this.btnResetDefault);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOk);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.chkCleanupSentMessages);
      this.Controls.Add(this.chkAskConfirmation);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormParams";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Paramètres Indicateur IA.Docs";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private CheckBox chkAskConfirmation;
    private CheckBox chkCleanupSentMessages;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Button btnOk;
    private Button btnCancel;
    private Button btnResetDefault;
    private TextBox txtRecipient;
    private TextBox txtPrefix;
    private TextBox txtCategory;
    private TextBox txtLogFilename;
  }
}