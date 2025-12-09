using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;

using SMA.Support;

using Outlook = Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;

namespace SMA.Process {
  public class TProcess : IProcess {

    const string ERROR_SUBJECT_MISSING = "[Subject is missing]";

    private readonly Outlook.Application Application;
    // be tolerant: return null rather than throwing if no explorer available
    private Explorer ActiveExplorer => Application?.ActiveExplorer();
    private MAPIFolder SentMailFolder => Application?.Session == null ? null : Application.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);

    private readonly ILogger Logger;
    private readonly IParameters Parameters;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TProcess(Outlook.Application application, ILogger logger, IParameters parameters) {
      Application = application;
      Logger = logger;
      Parameters = parameters;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public bool SendToIndicator() {
      var explorer = ActiveExplorer;
      if (explorer == null) {
        Logger.LogError("No active Outlook explorer found. Operation cancelled.");
        return false;
      }

      List<MailItem> SelectedMailItems = explorer.Selection.OfType<MailItem>().ToList();

      if (!SelectedMailItems.Any()) {
        MessageBox.Show("Vous devez sélectionner un ou plusieurs messages ...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      foreach (MailItem MailItemItem in SelectedMailItems) {

        try {
          if (IsIndicated(MailItemItem)) {
            Logger.LogWarning($"Le message {MailItemItem.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING} est déjà indicaté");
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
        } finally {
          // release the reference to the selected item
          ReleaseCom(MailItemItem);
        }
      }

      if (Parameters.CleanupSentMessages) {
        CleanupSentItems();
        Logger.LogInfo("SentItems has been cleaned up");
      }

      return true;
    }

    // rest unchanged (keep COM release improvements you already added)
    public int CleanupSentItems() {
      Logger.LogInfo("Cleaning up SentItems folder ...");
      int Counter = 0;
      try {
        Items sentItems = null;
        object currentObj = null;
        MailItem currentMail = null;
        MailItem nextMail = null;
        try {
          sentItems = SentMailFolder.Items;
          sentItems.Sort("[SentOn]", true);

          const int MAX_WORK_ITEMS = 50;
          int workCounter = 0;

          currentObj = sentItems.GetFirst();
          currentMail = currentObj as MailItem;

          while (currentMail != null && workCounter < MAX_WORK_ITEMS) {
            workCounter++;
            try {
              if (IsIndicated(currentMail)) {
                Logger.LogInfo($"Removing {currentMail.Subject?.WithQuotes() ?? ERROR_SUBJECT_MISSING} from SentItems");
                try {
                  currentMail.Delete();
                  Logger.LogInfo("  OK");
                  Counter++;
                } catch (System.Exception ex) {
                  Logger.LogError("  Unable to remove message", ex);
                }
              }
            } finally {
              // move to next before releasing current
              object nextObj = sentItems.GetNext();
              nextMail = nextObj as MailItem;

              // release current COM object
              ReleaseCom(currentMail);
              if (currentObj != null && !Object.ReferenceEquals(currentObj, currentMail)) {
                ReleaseCom(currentObj);
              }

              currentObj = nextObj;
              currentMail = nextMail;
              nextMail = null;
            }
          }
        } finally {
          ReleaseCom(currentMail);
          ReleaseCom(nextMail);
          ReleaseCom(currentObj);
          ReleaseCom(sentItems);
          ReleaseCom(SentMailFolder);
        }

        Logger.LogInfo($"Total {Counter} message(s) removed from SentItems");
        return Counter;
      } catch (System.Exception ex) {
        Logger.LogError("Unable to remove messages from SentItems", ex);
        return Counter;
      }
    }

    private void SendMailAsAttachment(MailItem MailItemItem) {
      MailItem NewMailItem = null;
      try {
        NewMailItem = (MailItem)Application.CreateItem(OlItemType.olMailItem);
        NewMailItem.Subject = $"{Parameters.Prefix}{MailItemItem.Subject ?? ERROR_SUBJECT_MISSING}";
        NewMailItem.To = Parameters.Recipient;
        // attach the mail item by value
        NewMailItem.Attachments.Add(MailItemItem, OlAttachmentType.olByValue, Type.Missing, Type.Missing);
        NewMailItem.Body = string.Empty;
        MarkAsIndicated(NewMailItem);

        // if sending many messages, consider throttling (small delay)
        NewMailItem.Send();
      } finally {
        // release the created mail
        ReleaseCom(NewMailItem);
      }
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

    private void ReleaseCom(object comObj) {
      if (comObj == null) {
        return;
      }
      try {
        if (Marshal.IsComObject(comObj)) {
          Marshal.FinalReleaseComObject(comObj);
        }
      } catch {
        try {
          Marshal.ReleaseComObject(comObj);
        } catch {
          // swallow any release exceptions to avoid crashing Outlook
        }
      }
    }

  }
}
