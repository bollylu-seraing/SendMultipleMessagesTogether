using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;
using SendMultipleMessagesTogether.Process;
using SendMultipleMessagesTogether.Support;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace SendMultipleMessagesTogether {
  public partial class RibbonSMMAA {


    public void button1_Click(object sender, RibbonControlEventArgs e) {

      IProcess Process = new TProcess(Globals.ThisAddIn.Application, ThisAddIn.Logger, ThisAddIn.Parameters);
      Process.Execute();
      
    }
  }
}
