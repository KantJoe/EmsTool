using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace org.Utils
{
    public class ZipUtil
    {
        public static List<string> UnzipFile(string fileName, string targetFolder)
        {
            var result = new List<string>();
            if (!File.Exists(fileName))
            {
                return result;
            }

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(fileName))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string fullPath = Path.Combine(targetFolder, entry.FullName);
                        result.Add(fullPath);
                        if (!entry.Name.EndsWith("/"))
                        {
                            // 确保不是目录，如果是目录则创建目录结构即可，无需解压文件内容到磁盘。
                            // true会覆盖已存在的文件。
                            entry.ExtractToFile(fullPath, true);
                        }
                        else if (!Directory.Exists(fullPath))
                        {
                            // 如果是目录，则创建目录。
                            Directory.CreateDirectory(fullPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "ZipUtil.UnzipFile {f},{t}", fileName, targetFolder);
                result.Clear();
            }

            return result;
        }
    }
}
