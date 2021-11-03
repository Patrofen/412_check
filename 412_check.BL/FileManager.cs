using System.Data;
using System.IO;
using System.Text;
using ExcelDataReader;

namespace _412_check.BL
{
    public interface IFileManager
    {
        DataTableCollection GetContent(string filePath);
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
        
        public DataTableCollection GetContent(string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });
                    DataTableCollection tableCollection = result.Tables;
                    return tableCollection;
                }
            }
        }
    }
}
