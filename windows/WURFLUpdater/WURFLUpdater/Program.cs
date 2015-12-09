using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace WURFLUpdater
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 2)
            {
                Console.Error.WriteLine("ScientiaMobile WURFL Snapshot Updater");
                Console.Error.WriteLine("This utility is used to update the WURFL.xml device database file.");
                Console.Error.WriteLine("Note that the file format (zip or xml.gz) is determined by the URL.");
                Console.Error.WriteLine("");
                Console.Error.WriteLine("usage: ./wurfl-updater.exe <url> <download_path>");
                Console.Error.WriteLine("  url            The WURFL Snapshot URL from your customer vault on scientiamobile.com");
                Console.Error.WriteLine("                 ex: https://data.scientiamobile.com/xxxxx/wurfl.zip");
                Console.Error.WriteLine("");
                Console.Error.WriteLine("  download_path  The directory to place the WURFL file into.");
                Console.Error.WriteLine("");
                Environment.Exit(1);
                
            }

            String remoteUrl = args[0];
            String localDir = args[1].TrimEnd('\\','/');
            String localFileName = localDir + Path.DirectorySeparatorChar + remoteUrl.Split('/').Last();
            DateTime lastModified = File.Exists(localFileName) ? File.GetLastWriteTime(localFileName) : DateTime.Now.AddDays(-7.0);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(remoteUrl);
            request.Timeout = 10000;
            request.AllowWriteStreamBuffering = false;
            request.UserAgent = "WURFL Updater/dotNet";
            request.IfModifiedSince = lastModified;
            
            Console.WriteLine("Downloading WURFL file...");

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                FileStream fileStream = new FileStream(localFileName, FileMode.Create);
                byte[] read = new byte[4096];
                int count = streamResponse.Read(read, 0, read.Length);
                while (count > 0)
                {
                    fileStream.Write(read, 0, count);
                    count = streamResponse.Read(read, 0, read.Length);
                }
                fileStream.Close();
                File.SetLastWriteTime(localFileName, response.LastModified);
                streamResponse.Close();
                response.Close();
                Console.WriteLine("Successfully downloaded WURFL to: " + localFileName);
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotModified)
                    {
                        Console.WriteLine("The WURFL File is up to date.");
                    }
                    else
                    {
                        Console.Error.WriteLine("Unexpected status code: HTTP {0}: {1}",
                            (int)((HttpWebResponse)e.Response).StatusCode,
                            ((HttpWebResponse)e.Response).StatusDescription
                        );
                        Environment.Exit(2);
                    }
                }
                else
                {
                    Console.Error.WriteLine("Unexpected Web Exception " + e.Message);
                    Environment.Exit(3);
                }
            }
        }
    }
}

