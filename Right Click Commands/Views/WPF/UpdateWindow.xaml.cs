using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Updater;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Right_Click_Commands.Views.WPF
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        //  Constants
        //  =========

        private const string DownloadFolderName = "Right-Click Commands";
        private const string DownloadFileName = "Update.msi";

        //  Variables
        //  =========

        private static readonly string NL = Environment.NewLine;
        private readonly IMessagePrompt messagePrompt;
        private WebClient webClient;

        private readonly string downloadFolder;
        private readonly string downloadLocation;

        private readonly UpdateFound updateFound;
        private readonly UpdateDownloading updateDownloading;

        //  Properties
        //  ==========

        public Asset Asset { get; set; }

        //  Constructors
        //  ============

        public UpdateWindow(IMessagePrompt messagePrompt, Asset asset)
        {
            downloadFolder = Path.Combine(Path.GetTempPath(), DownloadFolderName);
            downloadLocation = Path.Combine(downloadFolder, DownloadFileName);

            this.messagePrompt = messagePrompt;
            Asset = asset;

            InitializeComponent();

            updateFound = new UpdateFound(this);
            updateDownloading = new UpdateDownloading(this);

            MainControl.Content = updateFound;
        }

        //  Events
        //  ======

        public void VisitGithub(RoutedEventArgs e)
        {
            Topmost = false;
            try
            {
                Process.Start(Asset.UpdateURL);
            }
            catch
            {
                messagePrompt.PromptOK($"Unable to open webpage. Update can be found at:{NL}{Asset.UpdateURL}",
                    "Error", MessageType.Error);
            }
        }

        public void Yes(RoutedEventArgs e)
        {
            Topmost = false;
            MainControl.Content = updateDownloading;
        }

        public void No(RoutedEventArgs e)
        {
            Close();
        }

        public void Cancel(RoutedEventArgs e)
        {
            if (webClient != null)
            {
                webClient.CancelAsync();
            }
            No(e);
        }

        public void Install(RoutedEventArgs e)
        {
            Process.Start(downloadLocation);
            Environment.Exit(0);
        }

        public void Download(RoutedEventArgs e)
        {
            Directory.CreateDirectory(downloadFolder);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += Client_DownloadFileCompleted;
                webClient.DownloadProgressChanged += Client_DownloadProgressChanged;
                webClient.DownloadFileAsync(new Uri(Asset.URL), downloadLocation);
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
             updateDownloading.DownloadPercentage = e.ProgressPercentage;
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                updateDownloading.InstallButtonIsVisible = true;
                updateDownloading.Label = "Done!";
            }
            else if (e.Error != null)
            {
                MessageResult result = messagePrompt.PromptOKCancel("Unable to download the update!\nWould you like to download it in your web browser instead?", "Download Failure", MessageType.Error);
                switch (result)
                {
                    case MessageResult.OK:
                        Process.Start(Asset.UpdateURL);
                        Environment.Exit(0);
                        break;
                    default:
                        Close();
                        break;
                }
            }
        }
    }
}
