using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;
using System.IO;

namespace AE_DataTracker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        CollectCsvAttachments();
    }


    private const string email = "luxlupus1997@gmail.com";
    private const string appPassword = "ydbi vrdr lkul yggq";
    private const string saveDirectory = @"C:\CSV_Dump\";

    public void CollectCsvAttachments()
    {
        using (var client = new ImapClient())
        {
            client.Connect("imap.gmail.com", 993, true);
            client.Authenticate(email, appPassword);

            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadWrite);

            var query = SearchQuery.DeliveredAfter(DateTime.Now.AddDays(-1))
                                   .And(SearchQuery.SubjectContains("RunData"));

            foreach (var uid in inbox.Search(query))
            {
                var message = inbox.GetMessage(uid);

                foreach (var attachment in message.Attachments)
                {
                    if (attachment is MimePart part && part.FileName.EndsWith(".csv"))
                    {
                        var filePath = System.IO.Path.Combine(saveDirectory, part.FileName);
                        using (var stream = File.Create(filePath))
                        {
                            part.Content.DecodeTo(stream);
                        }
                        Console.WriteLine($"Saved CSV: {filePath}");
                    }
                }
            }

            client.Disconnect(true);
        }
    }


}