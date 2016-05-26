using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AESxWin.Helpers
{
    public static class FilesHelper
    {


        public static async Task EncryptFileAsync(this string path, string password)
        {
            await Task.Run(() =>
            {
                SharpAESCrypt.SharpAESCrypt.Encrypt(password, path, path + ".aes");
            });
        }
        public static async Task EncryptFilesAsync(this IEnumerable<string> paths, string password)
        {
            await Task.Run(async () =>
            {
                foreach (var path in paths)
                {
                    await path.EncryptFileAsync(password);
                    TextBoxLogHelper.Log(path + " Encrypted.");

                }
            });
        }

        public static async Task DecryptFileAsync(this string path, string password)
        {
            var outputpath = path.RemoveExtension();
            await Task.Run(() =>
            {

                SharpAESCrypt.SharpAESCrypt.Decrypt(password, path, outputpath);
            });
        }

        public static string RemoveExtension(this string path)
        {
            var outputpath = Path.ChangeExtension(path, "").TrimEnd(new char[] { '.' });
            return outputpath;
        }

        public static async Task DecryptFilesAsync(this IEnumerable<string> paths, string password)
        {
            await Task.Run(async () =>
            {
                foreach (var path in paths)
                {
                    await path.DecryptFileAsync(password);

                }
            });
        }


        public static IEnumerable<string> GetFolderFilesPaths(this string folder, bool followSubDirs = true)
        {
            var paths = new List<string>();
            if (!Directory.Exists(folder))
                return paths;
            if (followSubDirs)
            {
                var subFolders = Directory.GetDirectories(folder);
                if (subFolders != null)
                {

                    foreach (var path in subFolders)
                    {
                        paths.AddRange(GetFolderFilesPaths(path));

                    }

                }
            }
            var subFiles = Directory.GetFiles(folder);
            if (subFiles != null)
            {
                paths.AddRange(subFiles);
            }


            return paths;
        }

        public static IEnumerable<string> ParseExtensions(this string extentions)
        {
            var exts = new List<string>();
            var type = Regex.Match(extentions, @"\(.+\)").Value;
            if (!String.IsNullOrWhiteSpace(type))
                extentions = extentions.Replace(type, String.Empty);

            var matches = Regex.Matches(extentions, @"\b(\w+)\b");

            foreach (var ext in matches)
            {
                exts.Add("." + ext.ToString());
                
            }

            return exts;
        }


        public static bool CheckExtension(this string path, IEnumerable<string> extensions)
        {
            if (extensions == null)
                return true;
            if (extensions.Count() == 0)
                return true;

            foreach (var ext in extensions)
            {
                if (path.ToLower().EndsWith(ext.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
