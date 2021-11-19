using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace _412_check.BL
{
    public interface IModel
    {
        DataTableCollection GetContent(string filePath);
        void LoadTemplate(string source, string destination, string claimType);
        bool IsExist(string filePath);
    }
    public class Model : IModel
    {
        //Конструктор класса Model. Заполнение списков значениями из справочников.
        public Model() { }

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

        public void LoadTemplate(string source, string destination, string claimType)
        {
            //Установка путей к файлам 
            FileInfo xlFile_template = Utils.GetFileInfo("Шаблоны", source);
            FileInfo xlFile_system = Utils.GetFileInfo("Системные", destination);

            using (ExcelPackage template_package = new ExcelPackage(xlFile_template))
            {
                ExcelWorksheet worksheet_source = template_package.Workbook.Worksheets[1];
                using (ExcelPackage system_package = new ExcelPackage(xlFile_system))
                {
                    ExcelWorksheet worksheet_destination = system_package.Workbook.Worksheets[1];

                    int startRow = 2;
                    int lastRowDestination = worksheet_destination.Dimension.End.Row;

                    if (lastRowDestination > 2)
                    {
                        worksheet_destination.DeleteRow(startRow, lastRowDestination);
                    }

                    int lastRowSource = worksheet_source.Dimension.End.Row;

                    for (int row = startRow; row <= lastRowSource; row++)
                    {
                        worksheet_destination.Cells[row, 1].Value = worksheet_source.Cells[row, 1].Value;
                        worksheet_destination.Cells[row, 2].Value = worksheet_source.Cells[row, 2].Value;
                        worksheet_destination.Cells[row, 3].Value = worksheet_source.Cells[row, 3].Value;
                        worksheet_destination.Cells[row, 4].Value = worksheet_source.Cells[row, 4].Value;
                        worksheet_destination.Cells[row, 5].Value = worksheet_source.Cells[row, 5].Value;
                        worksheet_destination.Cells[row, 6].Value = claimType;

                        //Проверка формата даты
                        if (worksheet_source.Cells[row, 6].Value is DateTime)
                        {
                            worksheet_destination.Cells[row, 7].Value = worksheet_source.Cells[row, 6].Value;
                            worksheet_destination.Cells[row, 7].Style.Numberformat.Format = "dd.MM.yyyy";
                        }
                        else
                        {
                            if (DateTime.TryParse(worksheet_source.Cells[row, 6].Value.ToString(), out DateTime dDate))
                            {
                                worksheet_destination.Cells[row, 7].Value = dDate;
                                worksheet_destination.Cells[row, 7].Style.Numberformat.Format = "dd.MM.yyyy";
                            }
                            else
                            {
                                worksheet_destination.Cells[row, 7].Value = worksheet_source.Cells[row, 6].Value;
                            }
                        }

                        //Расчет срока погашения

                    }
                    system_package.Save();
                    lastRowDestination = worksheet_destination.Dimension.End.Row;
                    MessageBox.Show($"Количество загруженных строк: " + (lastRowDestination - 1).ToString(), "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
            } // the using statement automatically calls Dispose() which closes the package.
        }

    }
}
