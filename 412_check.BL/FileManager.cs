using System.IO;
using System.Text;

namespace _412_check.BL
{
    public interface IFileManager
    {
        string GetContent(string filePath);
        string GetContent(string filePath, Encoding encoding);
        void SaveContent(string content, string filepath);
        void SaveContent(string content, string filepath, Encoding encoding);
        int GetSymbolCount(string content);
        bool IsExist(string filePath);
    }
    public class FileManager:IFileManager
    {
        private readonly Encoding _defaultencoding = Encoding.GetEncoding(1251);
        public bool IsExist(string filePath)
        {
            bool isExist = File.Exists(filePath);
            return isExist;
        }
        public string GetContent(string filePath)
        {
            return GetContent(filePath, _defaultencoding);
        }
        public string GetContent(string filePath, Encoding encoding)
        {
            string content = File.ReadAllText(filePath, encoding);
            return content;
        }
        public void SaveContent(string content, string filepath)
        {
            SaveContent(content, filepath, _defaultencoding);
        }
        public void SaveContent(string content, string filepath, Encoding encoding)
        {
            File.WriteAllText(filepath, content, encoding);
        }
        public int GetSymbolCount(string content)
        {
            int count = content.Length;
            return count;
        }
    }
}
