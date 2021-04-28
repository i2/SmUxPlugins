using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using PluginSystem.API;

namespace PluginSystem.Helpers
{
    public static class WebHelper
    {
        public static string GetPageAsString(string url, int timeOut)
        {
            return GetPageAsString(url, timeOut, null);
        }

        public static string GetPageAsString(string url, int timeOut, string userAgent)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeOut;
            request.UserAgent = userAgent;
            var response = (HttpWebResponse)request.GetResponse();
            Encoding encoding = Encoding.GetEncoding(response.CharacterSet);
            return StreamHelper.StreamToString(response.GetResponseStream(), encoding);
        }

        public static string ExtractTextFromHtmlPage(string url, string before, string after)
        {
            return ExtractTextFromHtmlPage(url, before, after, null, 5000);
        }

        public static string ExtractTextFromHtmlPage(string url, string before, string after, string userAgent, int timeOut)
        {
            string pageAsString = GetPageAsString(url, timeOut, userAgent);
            return StringHelper.GetTextBetween(pageAsString, before, after);
        }

        public static void DownloadToFile(string url, string outputFileName)
        {
            Stream stream = DownloadToStream(url);
            StreamHelper.WriteStreamToFile(stream, outputFileName);
        }

        public static Stream DownloadToStream(string url)
        {
            WebRequest request = WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }

        public static Image ImageFromUrl(string url)
        {
            return Image.FromStream(DownloadToStream(url));
        }
    }
}
