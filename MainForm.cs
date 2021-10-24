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
    public interface IMainForm
    {
        string FilePath { get; }
        string Content { get; set; }
        void SetSymbolCount(int count);
        event EventHandler FileOpenClick;
        event EventHandler FileSaveClick;
        event EventHandler ContentChanged;
    }
    public partial class MainForm : Form, IMainForm
    {
        public MainForm()
        {
            InitializeComponent();
            butOpenFile.Click += butOpenFile_Click;
            butSaveFile.Click += butSave_Click;
            fldContent.TextChanged += fldContent_TextChanged;
            butSelectFile.Click += butSelectFile_Click;
        }
        #region Проброс событий
        private void butOpenFile_Click(object sender, EventArgs e)
        {
            if (FileOpenClick != null) FileOpenClick(this, EventArgs.Empty);
        }
        private void butSave_Click(object sender, EventArgs e)
        {
            if (FileSaveClick != null) FileSaveClick(this, EventArgs.Empty);
        }
        private void fldContent_TextChanged(object sender, EventArgs e)
        {
            if (ContentChanged != null) ContentChanged(this, EventArgs.Empty);
        }
        #endregion

        #region Реализация IMainForm
        public string FilePath
        {
            get { return fldFilePath.Text; }
        }
        public string Content
        {
            get { return fldContent.Text; }
            set { fldContent.Text = value; }
        }
        public void SetSymbolCount(int count)
        {
            lblSymbolCount.Text = count.ToString();
        }
        public event EventHandler FileOpenClick;
        public event EventHandler FileSaveClick;
        public event EventHandler ContentChanged;
        #endregion
        private void butSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fldFilePath.Text = dlg.FileName;
                if (FileOpenClick != null) FileOpenClick(this, EventArgs.Empty);
            }
        }

        private void CboSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = tableCollection[cboSheet.SelectedItem.ToString()];

            //EnumerableRowCollection<DataRow> query =
            //    from line in dt.AsEnumerable()
            //    where line.Field<string>("Текст") == "Три"
            //    orderby line.Field<int>("Номер")
            //    select line;
            //DataView view = query.AsDataView();

            //DataView view = dt.AsDataView();
            //dataGridView1.DataSource = view;

            dataGridView1.DataSource = dt;

            //изменить цвет ячейки
            dataGridView1.Rows[0].Cells[0].Style.BackColor = Color.Red;
        }

        DataTableCollection tableCollection;

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog openFileDialog = new OpenFileDialog() {Filter= "Excel Workbook|*.xlsx|Excel 97-2003 Workbook|*.xls" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilename.Text = openFileDialog.FileName;
                    using(var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using(IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() 
                            {
                                ConfigureDataTable=(_)=> new ExcelDataTableConfiguration() { UseHeaderRow=true}
                            });
                            tableCollection = result.Tables;
                            cboSheet.Items.Clear();
                            foreach (DataTable table in tableCollection)
                            {
                                cboSheet.Items.Add(table.TableName); //add sheet to combobox
                                cboSheet.SelectedItem = cboSheet.Items[0];
                            }
                        }
                    }
                }
            }
        }
    }
}
