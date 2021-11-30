using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using _412.Messages;


namespace _412_check.BL
{
    public interface IBusinessLogic
    {
        DataTableCollection ExcelToDataTable(FileInfo filePath);
        void DataTableToExcel(System.Data.DataTable content, FileInfo destination);
        void OpenXlUsingInterop(FileInfo filePath);
        void ResetSystemTable(System.Data.DataTable table);
        bool IsExist(string filePath);
        string FindCorrespondence(string obj, DataTable fromTable, string SearchedField, string ReturnedField, out int errors);
        double FindDouble(string obj, DataTable fromTable, string SearchedField, string ReturnedField, out int errors);
    }

    public class BusinessLogic : IBusinessLogic
    {
        //Конструктор класса Model. Заполнение списков значениями из справочников.
        public BusinessLogic() { }
        public bool IsExist(string filePath)
        {
            bool isExist = File.Exists(filePath);
            return isExist;
        }

        #region Взаимодействие с Excel
        //Используется Microsoft.Office.Interop.Excel
        public void OpenXlUsingInterop(FileInfo filePath)
        {
            Excel.Application excel = new Excel.Application();

            excel.Workbooks.Open(filePath.FullName);
            excel.Visible = true;
        }

        //Используется ExcelDataReader
        public DataTableCollection ExcelToDataTable(FileInfo filePath)
        {
            try
            {
                using (var stream = File.Open(filePath.FullName, FileMode.Open, FileAccess.Read))
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
            catch (Exception ex)
            {
                MessageBox.Show("ExcelToDataTable \n" + ex.Message);

                Excel.Workbook myExcelWorkbook = System.Runtime.InteropServices.Marshal.BindToMoniker(filePath.FullName) as Excel.Workbook;
                Excel.Application exApp = myExcelWorkbook.Application;
                exApp.Quit();
                return null;
            }
        }
        //Используется EPPlus
        public void DataTableToExcel(System.Data.DataTable content, FileInfo destination_xlFile)
        {
            try
            {
                using (ExcelPackage toExcel = new ExcelPackage(destination_xlFile))
                {
                    ExcelWorksheet wksht_destination = toExcel.Workbook.Worksheets[1];
                    wksht_destination.Cells["A2"].LoadFromDataTable(content, false);
                    wksht_destination.Cells[2, 7, content.Rows.Count + 1, 7].Style.Numberformat.Format = DateTimePickerFormat.Short.ToString();
                    toExcel.SaveAs(destination_xlFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        #endregion

        

        public void ResetSystemTable(System.Data.DataTable table)
        {
            switch (table.TableName)
            {
                case "debitorsSystem":
                case "commonRepoSystem":
                    table.Reset();
                    table.Columns.Add("Идентификатор требования к дебитору", typeof(string));
                    table.Columns.Add("Наименование дебитора", typeof(string));
                    table.Columns.Add("Код валюты", typeof(string));
                    table.Columns.Add("Объем требования в единицах валюты", typeof(double));
                    table.Columns.Add("Объем требования в рублях", typeof(double));
                    table.Columns.Add("Тип требования", typeof(string));
                    table.Columns.Add("Срок погашения", typeof(string));
                    break;
                case "currRatesSystem":
                    table.Reset();
                    table.Columns.Add("Цифр. код", typeof(int));
                    table.Columns.Add("Букв. код", typeof(string));
                    table.Columns.Add("Единиц", typeof(int));
                    table.Columns.Add("Валюта", typeof(string));
                    table.Columns.Add("Курс", typeof(double));
                    break;
            }
        }

        public string FindCorrespondence(string obj, DataTable fromTable, string SearchedField, string ReturnedField, out int errors)
        {
            errors = 0;
            try
            {
                var result = from row in fromTable.AsEnumerable()
                             where row.Field<string>(SearchedField) == obj
                             select row.Field<string>(ReturnedField);
                return result.First<string>();
            }
            catch (Exception)
            {
                errors += 1;
                return "error";
            }
        }

        public double FindDouble(string obj, DataTable fromTable, string SearchedField, string ReturnedField, out int errors)
        {
            errors = 0;
            try
            {
                var result = from row in fromTable.AsEnumerable()
                             where row.Field<string>(SearchedField) == obj
                             select row.Field<double>(ReturnedField);
                return result.First<double>();
            }
            catch (Exception)
            {
                errors += 1;
                return 0;
            }
        }
    }
}
