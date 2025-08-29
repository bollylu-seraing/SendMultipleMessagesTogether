using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;

using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace SendMultipleMessagesTogether {
  public partial class RibbonSMMAA {

    private ILogger Logger => ThisAddIn.Parameters.Logger;

    private void Ribbon1_Load(object sender, RibbonUIEventArgs e) {
      Logger.Log("The Ribbon is loaded.");
    }

    public void button1_Click(object sender, RibbonControlEventArgs e) {

      IEnumerable<MailItem> SelectedMailItems = Globals.ThisAddIn.Application.ActiveExplorer().Selection.OfType<MailItem>();

      try {
        if (!SelectedMailItems.Any()) {
          Logger.Log("Vous devez sélectionner un ou plusieurs messages ...");
          return;
        }

        // Now we can process the selected mail items
        foreach (MailItem MailItem in SelectedMailItems) {
          Logger.Log($"Processing {MailItem.Subject}");
        }
      } catch (System.Exception ex) {
        Logger.LogError("Error processing", ex);
      }
    }
  }
}
