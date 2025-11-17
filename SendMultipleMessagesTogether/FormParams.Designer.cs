using System;
using System.Windows.Forms;

namespace SendMultipleMessagesTogether
{
    partial class FormParams
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblRecipient = new System.Windows.Forms.Label();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblRecipient
            // 
            this.lblRecipient.AutoSize = true;
            this.lblRecipient.Location = new System.Drawing.Point(12, 9);
            this.lblRecipient.Name = "lblRecipient";
            this.lblRecipient.Size = new System.Drawing.Size(58, 13);
            this.lblRecipient.TabIndex = 0;
            this.lblRecipient.Text = "Récipient :";
            // 
            // lblPrefix
            // 
            this.lblPrefix.AutoSize = true;
            this.lblPrefix.Location = new System.Drawing.Point(12, 33);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(45, 13);
            this.lblPrefix.TabIndex = 0;
            this.lblPrefix.Text = "Prefixe :";
            // 
            // FormParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblRecipient);
            this.Controls.Add(this.lblPrefix);
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

        private Label lblRecipient;
        private Label lblPrefix;
        private Label lblCategory = new Label() { Text = "Catégorie :" };
        private Label lblLogFilename = new Label() { Text = "Fichier log :" };
        private Label lblAskConfirmation = new Label() { Text = "Confirmation pour indication" };
        private Label lblCleanupSentMessage = new Label() { Text = "Nettoyer les messages indicatés dans les éléments envoyés" };

        private TextBox txtRecipient = new TextBox();
        private TextBox txtPrefix = new TextBox();
        private TextBox txtCategory = new TextBox();
        private TextBox txtLogFilename = new TextBox();
        private CheckBox chkAskConfirmation = new CheckBox();
        private CheckBox chkCleanupSentMessages = new CheckBox();

        private Button btnOk = new Button();
        private Button btnCancel = new Button();
        private Button btnResetDefault = new Button();
    }
}