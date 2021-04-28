using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using PluginSystem.Helpers;
using System.Diagnostics;

namespace PluginSystem.Common
{
    public static class ZipHelper
    {
        public static void ExtractFiles(IProgress progress, string zipFilename, IDictionary<string, string> destinationPath)
        {
            using (Package zip = Package.Open(zipFilename, FileMode.Open))
            {
                PackagePartCollection packagePartCollection = zip.GetParts();

                int i = 0;
                int totalParts = packagePartCollection.Count();
                
                foreach (PackagePart part in packagePartCollection)
                {
                    progress.Message = string.Format("Rozpakowywanie plików: ({0}/{1})", i++, totalParts);
                    progress.ProgressValue = i*100/totalParts;
                    
                    string originalFileName = part.Uri.OriginalString.Substring(1).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                    string destinationFileName;

                    if (destinationPath.TryGetValue(originalFileName, out destinationFileName))
                    {
                        using (Stream stream = part.GetStream(FileMode.Open, FileAccess.ReadWrite))
                        {
                            StreamHelper.WriteStreamToFile(stream, destinationFileName);
                        }
                    }
                }
            }
        }

        public static void ExtractFile(string zipFilename, string archiveFileName, string destinationFileName)
        {
            using (Package zip = Package.Open(zipFilename, FileMode.Open))
            {
                PackagePartCollection packagePartCollection = zip.GetParts();
                foreach (PackagePart part in packagePartCollection)
                {
                    string originalFileName = part.Uri.OriginalString.Substring(1).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                    if (originalFileName.Equals(archiveFileName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (Stream stream = part.GetStream(FileMode.Open, FileAccess.ReadWrite))
                        {
                            StreamHelper.WriteStreamToFile(stream, destinationFileName);
                        }
                    }
                }
            }
        }

        public static void AddFileToZip(string zipFilename, string subfolderName, string fileToAdd)
        {
            using (Package zip = Package.Open(zipFilename, FileMode.OpenOrCreate))
            {
                const string DEST_FILE_NAME_FORMAT = ".{0}{1}{0}{2}";
                string destFileName = string.Format(DEST_FILE_NAME_FORMAT, new object[] { Path.DirectorySeparatorChar, subfolderName, Path.GetFileName(fileToAdd) });
                Uri uri = PackUriHelper.CreatePartUri(new Uri(destFileName, UriKind.Relative));
                if (zip.PartExists(uri))
                {
                    zip.DeletePart(uri);
                }
                PackagePart part = zip.CreatePart(uri, string.Empty, CompressionOption.Normal);
                using (var fileStream = new FileStream(fileToAdd, FileMode.Open, FileAccess.Read))
                {
                    using (Stream dest = part.GetStream())
                    {
                        StreamHelper.CopyStream(fileStream, dest);
                    }
                }
            }
        }

        public static void AddFilesToZip(IProgress progress, string zipFilename, string subfolder, List<string> list)
        {
            using (Package zip = Package.Open(zipFilename, FileMode.OpenOrCreate))
            {
                const string DEST_FILE_NAME_FORMAT = ".{0}{1}{0}{2}";

                int i = 1;

                foreach (string fileToAdd in list)
                {
                    progress.Message = string.Format("Pakowanie plików: {0} z {1}", i++, list.Count);
                    progress.ProgressValue = 100*(i-1)/list.Count;

                    string destFileName = string.Format(DEST_FILE_NAME_FORMAT, new object[] { Path.DirectorySeparatorChar, subfolder, Path.GetFileName(fileToAdd) });
                    Uri uri = PackUriHelper.CreatePartUri(new Uri(destFileName, UriKind.Relative));
                    if (zip.PartExists(uri))
                    {
                        zip.DeletePart(uri);
                    }

                    PackagePart part = zip.CreatePart(uri, string.Empty, CompressionOption.Normal);
                    using (var fileStream = new FileStream(fileToAdd, FileMode.Open, FileAccess.Read))
                    {
                        using (Stream dest = part.GetStream())
                        {
                            StreamHelper.CopyStream(fileStream, dest);
                        }
                    }                    
                }
                StatusMessage.Remove();
            }
        }

        public static IList<string> GetArchiveFileList(string zipFilename)
        {
            var result = new List<string>();

            using (Package zip = Package.Open(zipFilename, FileMode.Open))
            {
                PackagePartCollection packagePartCollection = zip.GetParts();
                foreach (PackagePart part in packagePartCollection)
                {
                    string originalFileName = part.Uri.OriginalString.Substring(1).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                    result.Add(originalFileName);
                }
            }

            return result;
        }
    }

    public static class Logger
    {
        private static StringBuilder m_Log;

        public static void CreateNewLog(string title)
        {
            m_Log = new StringBuilder();
            m_Log.AppendFormat(
                @"<html>
                    <head>
                        <meta http-equiv=""content-type"" content=""text/html; charset=UTF-8"" />
                        <title>{0}</title>
                    </head>
                    <body id=""content_body"">
            ", title);
        }

        public static void AddToLog(string line)
        {
            m_Log.Append(line);
        }

        public static void ShowLogResult()
        {
            m_Log.Append(@"</body></html>");
            string logFileName = Path.GetTempFileName()+".html";
            StringHelper.StringToFile(m_Log.ToString(), logFileName);
            Process.Start(logFileName);
        }
    }
}
