using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PluginSystem.Helpers
{
    public static class StringHelper
    {
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.UTF8);
        }

        public static string FileToString(string filePath, Encoding encoding)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                return StreamHelper.StreamToString(fileStream, encoding);
            }
        }

        public static void StringToFile(string content, string filePath)
        {
            StringToFile(content, filePath, Encoding.UTF8);
        }

        public static void StringToFile(string content, string filePath, Encoding encoding)
        {
            using (MemoryStream memoryStream = StreamHelper.StringToStream(content, encoding))
            {
                StreamHelper.WriteStreamToFile(memoryStream, filePath);
            }
        }

        public static string GetTextBetween(string orignalText, string before, string after)
        {
            int start = orignalText.IndexOf(before, StringComparison.Ordinal);
            int end = orignalText.IndexOf(after, StringComparison.Ordinal);
            
            if (start != -1 && end != -1 && end>start)
            {
                return orignalText.Substring(start + before.Length, end - start - before.Length);
            }
            
            return null;
        }

        public static string RemoveFromText(string text, string toRemove)
        {
            int index = text.IndexOf(toRemove);

            if (index != -1 )
            {
                text = text.Remove(index, toRemove.Length);
            }

            return text;
        }

        public static string XmlString(string s)
        {
            var builder = new StringBuilder(s);
            const string AMPERSAND_LABEL = "&";
            const string AMP_LABEL = "&amp;";
            builder.Replace(AMPERSAND_LABEL, AMP_LABEL);
            const string LESS_THAN_LABEL = "<";
            const string LT_LABEL = "&lt;";
            builder.Replace(LESS_THAN_LABEL, LT_LABEL);
            const string MORE_THAN_LABEL = ">";
            const string GT_LABEL = "&gt;";
            builder.Replace(MORE_THAN_LABEL, GT_LABEL);
            const string ZERO_LABEL = "\0";
            builder.Replace(ZERO_LABEL, String.Empty);

            return builder.ToString();
        }

        public static string PlainTextToHtml(string s)
        {
            string result = s.Replace("\r\n", "<br />");
            result = result.Replace((char)3, ' ');
            result = result.Replace("\x002", "<br />");
            result = result.Replace((char)1, ' ');
            result = result.Replace((char)0, ' ');
            return result;
        }

        public static string RemoveHtmlTags(string htmlText)
        {
            htmlText = htmlText.Replace("</P>", "\r\n");
            var objRegEx = new Regex("<[^>]*>");
            return objRegEx.Replace(htmlText, "");
        }

        private static string GetCharAsHexCode(char c)
        {
            const string HEX_FORMAT = "{0:x}";
            string hexadecimal = string.Format(HEX_FORMAT, (int)c);

            while (hexadecimal.Length < 4) hexadecimal = '0' + hexadecimal;
            return hexadecimal;
        }

        private static string GetCharAsRegexSymbol(char c)
        {
            const string REGEX_UNICODE_CHAR_FORMAT = @"\u{0}";
            return string.Format(REGEX_UNICODE_CHAR_FORMAT, GetCharAsHexCode(c));
        }

        public static string ToRegexpEscapedString(this string s)
        {
            var result = new StringBuilder(s.Length * 4);
            foreach (char c in s)
            {
                result.Append(GetCharAsRegexSymbol(c));
            }

            return result.ToString();
        }
    }
}