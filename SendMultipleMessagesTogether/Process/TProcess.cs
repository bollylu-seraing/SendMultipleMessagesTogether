using System;
using System.Collections.Generic;
using System.Drawing.Text;
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
    private Explorer ActiveExplorer => Application?.ActiveExplorer() ?? throw new ApplicationException("ActiveExplorer is (null)");
    private MAPIFolder SentMailFolder => Application?.Session?.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail) ?? throw new ApplicationException("Application is (null)");

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

          if (ThisAddIn.Parameters.WithConfirmation && MessageBox.Show($"Envoyer le message {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING} à l'Indicateur ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
            Logger.LogInfo($"Envoi du message {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING} annulé par l'utilisateur");
            return false;
          }

          SendMailAsAttachment(MailItemItem);
          Logger.LogInfo($"Processed {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING}");

          MarkAsIndicatedAndSave(MailItemItem);
          Logger.LogInfo($"Original {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING} is marked {Parameters.Category.WithQuotes()}");


        } catch (System.Exception ex) {
          Logger.LogError($"Error processing {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING}", ex);
          return false;
        }
      }

      if (Parameters.CleanupSentMessages) {
        CleanupSentItems();
        Logger.LogInfo("SentItems has been cleaned up");
      }

      return true;
    }

    private void SendMailAsAttachment(MailItem MailItemItem) {
      MailItem NewMailItem = (MailItem)Application.CreateItem(OlItemType.olMailItem);
      NewMailItem.Subject = $"{Parameters.Prefix}{MailItemItem.Subject ?? ERROR_SUBJECT_MISSING}";
      NewMailItem.To = Parameters.Recipient;
      NewMailItem.Attachments.Add(MailItemItem, OlAttachmentType.olByValue, Type.Missing, Type.Missing);
      NewMailItem.Body = string.Empty;
      MarkAsIndicated(NewMailItem);
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
    }

    private void MarkAsIndicatedAndSave(MailItem mailItem) {
      MarkAsIndicated(mailItem);
      mailItem.Save();
    }

    private void CleanupSentItems() {
      
      int Counter = 0;
      foreach (MailItem MailItemItem in SentMailFolder.Items.OfType<MailItem>()) {
        if (IsIndicated(MailItemItem)) {
          Logger.LogInfo($"Removing {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING} from SentItems");
          try {
            MailItemItem.Delete();
            Logger.LogInfo("  OK");
            Counter++;
          } catch (System.Exception ex) {
            Logger.LogError("  Unable to remove message", ex);
          }
        }
      }
      Logger.LogInfo($"Total {Counter} message(s) removed from SentItems");
    }
  }
}
