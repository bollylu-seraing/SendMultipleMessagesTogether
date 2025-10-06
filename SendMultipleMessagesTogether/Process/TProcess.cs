using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;

using SendMultipleMessagesTogether.Support;

using Outlook = Microsoft.Office.Interop.Outlook;


namespace SendMultipleMessagesTogether.Process {
  public class TProcess : IProcess {

    const string ERROR_SUBJECT_MISSING = "[Subject is missing]";

    private readonly Outlook.Application Application;
    private Explorer ActiveExplorer => Application.ActiveExplorer();
    private readonly ILogger Logger;
    private readonly IParameters Parameters;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TProcess(Outlook.Application application, ILogger logger, IParameters parameters) {
      Application = application ?? throw new ArgumentNullException(nameof(application));
      Logger = logger ?? throw new ArgumentNullException(nameof(logger));
      Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public bool Execute() {
      List<MailItem> SelectedMailItems = ActiveExplorer.Selection.OfType<MailItem>().ToList();

      if (!SelectedMailItems.Any()) {
        MessageBox.Show("Vous devez sélectionner un ou plusieurs messages ...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      foreach (MailItem MailItemItem in SelectedMailItems) {

        try {
          if (IsIndicated(MailItemItem)) {
            Logger.LogWarning($"Le message {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING} est déjà indicaté");
            //MessageBox.Show($"Le message {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING} est déjà indicaté", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            continue;
          }

          SendMailAsAttachment(MailItemItem);
          MarkAsIndicated(MailItemItem);

          Logger.Log($"Processed {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING}");

        } catch (System.Exception ex) {
          Logger.LogError($"Error processing {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING}", ex);
        }
      }

      return true;
    }

    private void SendMailAsAttachment(MailItem MailItemItem) {
      MailItem NewMailItem = (MailItem)Application.CreateItem(OlItemType.olMailItem);
      NewMailItem.Subject = $"{Parameters.Prefix}{MailItemItem.Subject ?? ERROR_SUBJECT_MISSING}";
      NewMailItem.To = Parameters.Recipient;
      NewMailItem.Attachments.Add(MailItemItem, OlAttachmentType.olByValue, Type.Missing, Type.Missing);
      NewMailItem.Body = string.Empty;
      NewMailItem.Send();
    }

    public Task<bool> ExecuteAsync() {
      throw new NotImplementedException();
    }

    static readonly char[] CATEGORIES_SPLIT_SEPARATOR = new char[] { ',' };
    const string CATEGORIES_JOIN_SEPARATOR = ",";

    private bool IsIndicated(MailItem mailItem) {
      List<string> CurrentCategories = mailItem.Categories?.Trim()?.Split(CATEGORIES_SPLIT_SEPARATOR)?.ToList() ?? new List<string>();
      return CurrentCategories.Any(c => c.Equals(Parameters.Category, StringComparison.InvariantCultureIgnoreCase));
    }

    private void MarkAsIndicated(MailItem mailItem) {
      List<string> CurrentCategories = mailItem.Categories?.Trim()?.Split(CATEGORIES_SPLIT_SEPARATOR)?.ToList() ?? new List<string>();
      CurrentCategories.Add(Parameters.Category);
      mailItem.Categories = string.Join(CATEGORIES_JOIN_SEPARATOR, CurrentCategories);
      mailItem.Save();
    }
  }
}
