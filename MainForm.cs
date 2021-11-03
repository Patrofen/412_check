using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _412_check
{
    public delegate void FormEventHandler(object sender, FormEventArgs e);

    public interface IMainForm
    {
        string FilePath { get; }
        object ExcelTable { get; set; }
        void SetSheetList(DataTableCollection dataTableCollection);
        event EventHandler FileOpenClick;
        //event EventHandler FileSaveClick;
        event FormEventHandler CboSheetSelectedIndexChanged;
    }
    public partial class MainForm : Form, IMainForm
    {
        public MainForm()
        {
            InitializeComponent();
        }
        #region Проброс событий
        private void butSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Excel Workbook|*.xlsx|Excel 97-2003 Workbook|*.xls";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fldFilePath.Text = dlg.FileName;
                if (FileOpenClick != null) FileOpenClick(this, EventArgs.Empty);
            }
        }
        //private void butSave_Click(object sender, EventArgs e)
        //{
        //    if (FileSaveClick != null) FileSaveClick(this, EventArgs.Empty);
        //}
        //private void fldContent_TextChanged(object sender, EventArgs e)
        //{
        //    if (ContentChanged != null) ContentChanged(this, EventArgs.Empty);
        //}
        private void CboSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormEventArgs SheetName = new FormEventArgs(cboSheet.SelectedItem.ToString());
            if (CboSheetSelectedIndexChanged != null) CboSheetSelectedIndexChanged(this, SheetName);

            //        DataTable dt = tableCollection[cboSheet.SelectedItem.ToString()];

            ////EnumerableRowCollection<DataRow> query =
            ////    from line in dt.AsEnumerable()
            ////    where line.Field<string>("Текст") == "Три"
            ////    orderby line.Field<int>("Номер")
            ////    select line;
            ////DataView view = query.AsDataView();

            ////DataView view = dt.AsDataView();
            ////dataGridView1.DataSource = view;

            //dataGridView1.DataSource = dt;

            ////изменить цвет ячейки
            //dataGridView1.Rows[0].Cells[0].Style.BackColor = Color.Red;
        }
        #endregion

        #region Реализация IMainForm
        public string FilePath
        {
            get { return fldFilePath.Text; }
        }
        public object ExcelTable
        {
            get { return dataGridView1.DataSource; }
            set { dataGridView1.DataSource = value; }
        }
        public event EventHandler FileOpenClick;
        //public event EventHandler FileSaveClick;
        public event FormEventHandler CboSheetSelectedIndexChanged;
        #endregion

        public void SetSheetList(DataTableCollection SheetsCollection)
        {
            cboSheet.Items.Clear();
            foreach (DataTable table in SheetsCollection)
            {
                cboSheet.Items.Add(table.TableName); //add sheet to combobox
            }
            cboSheet.SelectedItem = cboSheet?.Items[0];
        }

        //private void BtnBrowse_Click(object sender, EventArgs e)
        //{
        //    using(OpenFileDialog openFileDialog = new OpenFileDialog() {Filter= "Excel Workbook|*.xlsx|Excel 97-2003 Workbook|*.xls" })
        //    {
        //        if (openFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            fldFilePath.Text = openFileDialog.FileName;
        //            using(var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
        //            {
        //                using(IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
        //                {
        //                    DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() 
        //                    {
        //                        ConfigureDataTable=(_)=> new ExcelDataTableConfiguration() { UseHeaderRow=true}
        //                    });
        //                    tableCollection = result.Tables;
        //                    cboSheet.Items.Clear();
        //                    foreach (DataTable table in tableCollection)
        //                    {
        //                        cboSheet.Items.Add(table.TableName); //add sheet to combobox
        //                        cboSheet.SelectedItem = cboSheet.Items[0];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
