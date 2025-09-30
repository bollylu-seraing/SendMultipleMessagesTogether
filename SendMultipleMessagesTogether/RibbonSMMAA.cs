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
        foreach (MailItem MailItemItem in SelectedMailItems) {

          MailItem NewMailItem = (MailItem)Globals.ThisAddIn.Application.CreateItem(OlItemType.olMailItem);
          NewMailItem.Subject = $"{ThisAddIn.Parameters.Prefix}{MailItemItem.Subject}";
          NewMailItem.To = ThisAddIn.Parameters.Recipient;
          NewMailItem.Attachments.Add(MailItemItem, OlAttachmentType.olByValue, Type.Missing, Type.Missing);
          //NewMailItem.Display();
          NewMailItem.Send();

          string CurrentCategories = MailItemItem.Categories?.Trim() ?? string.Empty;
          MailItemItem.Categories = $"{CurrentCategories}{(CurrentCategories != "" ? ",":"")}{ThisAddIn.Parameters.Category}";
          MailItemItem.Save();

          Logger.Log($"Processing {MailItemItem.Subject}");
        }
      } catch (System.Exception ex) {
        Logger.LogError("Error processing", ex);
      }
    }
  }
}
