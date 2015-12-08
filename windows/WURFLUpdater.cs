using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace WURFLUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            string remoteUrl = "https://data.scientiamobile.com/xxxxx/wurfl.zip";
            string localFileName = @"C:\temp\wurfl.zip";
            DateTime lastModified = File.Exists(localFileName) ? File.GetLastWriteTime(localFileName) : DateTime.Now.AddDays(-7.0);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(remoteUrl);
            request.Timeout = 10000;
            request.AllowWriteStreamBuffering = false;
            request.UserAgent = "WURFL Downloader/dotNet";
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
                        Console.WriteLine("The WURFL File is up to date.");
                    else
                        Console.WriteLine("Unexpected status code = " + ((HttpWebResponse)e.Response).StatusCode);
                }
                else
                    Console.WriteLine("Unexpected Web Exception " + e.Message); 
            }
        }
    }
}
