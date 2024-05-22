using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter the URL to download the update:");
        string url = Console.ReadLine();

        if (string.IsNullOrEmpty(url))
        {
            Console.WriteLine("URL cannot be empty.");
            return;
        }

        string tempFilePath = Path.GetTempFileName();
        string extractPath = Path.Combine(Path.GetTempPath(), "ExtractedUpdate");

        try
        {
            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("Downloading...");
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(tempFilePath, fileBytes);
                Console.WriteLine("Download complete.");
            }

            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            Console.WriteLine("Extracting...");
            ZipFile.ExtractToDirectory(tempFilePath, extractPath, true);
            Console.WriteLine("Extraction complete.");

            string batchFilePath = Path.Combine(extractPath, "update.bat");
            if (File.Exists(batchFilePath))
            {
                Console.WriteLine("Running update script...");
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = batchFilePath,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
                Console.WriteLine("Script execution complete.");

                Console.WriteLine("Update complete. Please reboot your system.");
            }
            else
            {
                Console.WriteLine("No update.bat file found in the downloaded ZIP.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }
}
