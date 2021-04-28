using System;
using System.Collections.Generic;
using PluginSystem.Helpers;

namespace MediaFiles.Image
{
    public static class ImageDownloadHelper
    {
        public static IEnumerable<string> ParSeImageUrls(string url)
        {
            string htmlPage = WebHelper.GetPageAsString(url, 3000);

            const string IMAGE_HTML_CODE = "<img";
            const string IMAGE_SRC_CODE = @"src=""";

            int index = htmlPage.IndexOf(IMAGE_HTML_CODE);
            while (index != -1)
            {
                htmlPage = htmlPage.Substring(index);

                int brackedEnd = htmlPage.IndexOf('>');
                int start = htmlPage.IndexOf(IMAGE_SRC_CODE) + IMAGE_SRC_CODE.Length;
                int end = htmlPage.IndexOf('"', start + 1);

                if (end > start && start < brackedEnd)
                {
                    string loc = htmlPage.Substring(start, end - start);

                    string baseUrl = GetBaseUrl(url);
                    var fullImageUrl = GetFullImageUrl(baseUrl, loc);

                    yield return fullImageUrl;
                }

                index = IMAGE_HTML_CODE.Length < htmlPage.Length
                            ? htmlPage.IndexOf(IMAGE_HTML_CODE, IMAGE_HTML_CODE.Length)
                            : -1;
            }

            yield break;
        }

        private static string GetFullImageUrl(string baseUrl, string fullImageUrl)
        {
            if ((!fullImageUrl.StartsWith("http://") && !fullImageUrl.StartsWith("https://")) && baseUrl != String.Empty)
            {
                fullImageUrl = baseUrl + "/" + fullImageUrl.TrimStart('/');
            }
            return fullImageUrl;
        }

        private static string GetBaseUrl(string url)
        {
            int inx = url.IndexOf("://") + "://".Length;
            int end = url.IndexOf('/', inx);

            return end != -1 ? url.Substring(0, end) : String.Empty;
        }
    }
}