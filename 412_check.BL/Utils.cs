using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _412_check.BL
{
    public class Utils
    {
        static DirectoryInfo _outputDir = null;
        public static DirectoryInfo OutputDir
        {
            get
            {
                return _outputDir;
            }
            set
            {
                _outputDir = value;
                if (!_outputDir.Exists)
                {
                    _outputDir.Create();
                }
            }
        }

        public static FileInfo GetFileInfo(string xlDir, string file)
        {
            char dirSept = Path.DirectorySeparatorChar;
            var fi = new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}412_Tables" + dirSept + xlDir + dirSept + file);
            return fi;
        }

        internal static DirectoryInfo GetDirectoryInfo(string directory)
        {
            var di = new DirectoryInfo(_outputDir.FullName + Path.DirectorySeparatorChar + directory);
            if (!di.Exists)
            {
                di.Create();
            }
            return di;
        }
    }
}