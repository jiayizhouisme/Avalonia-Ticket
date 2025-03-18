using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvvm.Helpers
{
    public static class PathHelper
    {
        private static string _localFolder = string.Empty;
        private static string LocalFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(_localFolder))
                {
                    return _localFolder;
                }
                _localFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(Mvvm));
                if (!Directory.Exists(_localFolder))
                {
                    Directory.CreateDirectory(_localFolder);
                }
                return _localFolder;
            }
        }
        public static string GetLocalFolder(string fileName)
        {
            return Path.Combine(LocalFolder,fileName);
        }

        internal static string GetLocalFilePath(string dbName)
        {
            throw new NotImplementedException();
        }
    }
}
