using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace UpdateApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            string url = urlEntry.Text;
            if (string.IsNullOrWhiteSpace(url))
            {
                await DisplayAlert("Error", "Please enter a valid URL.", "OK");
                return;
            }

            string tempFilePath = Path.Combine(FileSystem.CacheDirectory, "update.zip");
            downloadProgressBar.IsVisible = true;

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                statusLabel.Text = "Status: Downloading...";
                try
                {
                    await webClient.DownloadFileTaskAsync(new Uri(url), tempFilePath);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error downloading file: {ex.Message}", "OK");
                    statusLabel.Text = "Status: Error";
                    downloadProgressBar.IsVisible = false;
                    return;
                }
            }

            string extractPath = Path.Combine(FileSystem.CacheDirectory, "update");
            Directory.CreateDirectory(extractPath);
            ZipFile.ExtractToDirectory(tempFilePath, extractPath, true);

            string batchFilePath = Path.Combine(extractPath, "update.bat");
            if (File.Exists(batchFilePath))
            {
                statusLabel.Text = "Status: Running updater...";
                downloadProgressBar.IsVisible = false;

                try
                {
                    Process batchProcess = new Process();
                    batchProcess.StartInfo.FileName = batchFilePath;
                    batchProcess.StartInfo.UseShellExecute = false;
                    batchProcess.StartInfo.RedirectStandardOutput = true;
                    batchProcess.StartInfo.RedirectStandardError = true;
                    batchProcess.Start();
                    await batchProcess.WaitForExitAsync();

                    statusLabel.Text = "Status: Update complete. Please reboot.";
                    await DisplayAlert("Update Complete", "Update complete. Please reboot your system.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error running update script: {ex.Message}", "OK");
                    statusLabel.Text = "Status: Error";
                }
            }
            else
            {
                await DisplayAlert("Error", "update.bat not found in the downloaded ZIP file.", "OK");
                statusLabel.Text = "Status: Error";
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadProgressBar.Progress = e.ProgressPercentage / 100.0;
            statusLabel.Text = $"Status: Downloading... {e.ProgressPercentage}%";
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                DisplayAlert("Error", $"Error downloading file: {e.Error.Message}", "OK");
                statusLabel.Text = "Status: Error";
                downloadProgressBar.IsVisible = false;
            }
            else
            {
                statusLabel.Text = "Status: Download complete";
            }
        }
    }
}
