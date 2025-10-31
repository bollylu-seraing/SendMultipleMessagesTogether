using Microsoft.Office.Core;
using SendMultipleMessagesTogether.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMultipleMessagesTogether {
  public partial class Form1 : Form {
    
    private readonly IParameters Parameters;
    public IParameters NewParameters { get; private set; }

    private ILogger Logger => Parameters.Logger;

    public Form1() {
      InitializeComponent();
    }

    public Form1(IParameters parameters) : this() {
      Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
    }

    
    private void Form1_Load(object sender, EventArgs e) {

      txtRecipient.Text = Parameters.Recipient;
      txtPrefix.Text = Parameters.Prefix;
      txtCategory.Text = Parameters.Category;
      txtLogFilename.Text = Parameters.LogFilename;
      chkAskConfirmation.Checked = Parameters.WithConfirmation;
      chkCleanupSentMessages.Checked = Parameters.CleanupSentMessages;

    }
  }
}
