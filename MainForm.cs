using System;
using System.Drawing;
using System.Windows.Forms;


namespace _412_check
{
    public delegate void FormEventHandler(object sender, FormEventArgs e);

    public interface IMainForm
    {
        DateTime ReportDate { get; set; }
        object DataGridView { get; set; }
        string TotalDebitorsLines { set; }
        string TotalDebitorsErrors { set; }
        string TotalRepoLines { set; }
        string TotalRepoErrors { set; }
        string TotalCurrRatesLines { set; }
        string TotalCurrRatesErrors { set; }
        string CboGridViewSelect { set; }


        event EventHandler LoadTemplatesClick;
        event EventHandler OpenDebitorsTmplateClick;
        event EventHandler OpenRevRepoTmplateClick;
        event EventHandler OpenCurrRatesTmplateClick;
        event EventHandler CalculateClick;
        event EventHandler CheckClick;
        event FormEventHandler CboGridViewChanged;
    }
    public partial class MainForm : Form, IMainForm
    {
        //Конструктор
        public MainForm()
        {
            InitializeComponent();
        }

        #region Проброс событий
        private void btnLoadTemplates_Click(object sender, EventArgs e)
        {
            if (LoadTemplatesClick != null) LoadTemplatesClick(this, EventArgs.Empty);
            btnCalculate.Enabled = true;
        }
        private void btnOpenDebTmpl_Click(object sender, EventArgs e)
        {
            if (OpenDebitorsTmplateClick != null) OpenDebitorsTmplateClick(this, EventArgs.Empty);
        }
        private void btnOpenRepoTmpl_Click(object sender, EventArgs e)
        {
            if (OpenRevRepoTmplateClick != null) OpenRevRepoTmplateClick(this, EventArgs.Empty);
        }
        private void btnOpenCurrRatesTmpl_Click(object sender, EventArgs e)
        {
            if (OpenCurrRatesTmplateClick != null) OpenCurrRatesTmplateClick(this, EventArgs.Empty);
        }
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (CalculateClick != null) CalculateClick(this, EventArgs.Empty);
            btnCheck.Enabled = true;
        }
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (CheckClick != null) CheckClick(this, EventArgs.Empty);
        }
        private void cboGridViewSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string message = cboGridViewSelect.SelectedItem.ToString();
            if (CboGridViewChanged != null) CboGridViewChanged(this, new FormEventArgs(message));
        }
        #endregion

        #region Реализация IMainForm
        private DateTime reportDate;
        public DateTime ReportDate
        {
            get { return reportDate; }
            set { reportDate = value; }
        }
        //Вывод в dataGridView информации из файла Excel
        public object DataGridView
        {
            get { return dataGridView1.DataSource; }
            set { dataGridView1.DataSource = value; }
        }
        //Количество строк Дебиторской задолженности
        public string TotalDebitorsLines
        {
            set { lblDebitorsLines.Text = value; }
        }
        //Количество ошибок загрузки Дебиторской задолженности
        public string TotalDebitorsErrors
        {
            set { lblDebitorsErrors.Text = value; }
        }
        //Количество строк Обратного РЕПО
        public string TotalRepoLines
        {
            set { lblRepoLines.Text = value; }
        }
        //Количество ошибок загрузки Обратного РЕПО
        public string TotalRepoErrors
        {
            set { lblRepoErrors.Text = value; }
        }
        //Количество строк Курсов валют
        public string TotalCurrRatesLines
        {
            set { lblCurrRatesLines.Text = value; }
        }
        //Количество ошибок загрузки Курсов валют
        public string TotalCurrRatesErrors
        {
            set { lblCurrRatesErrors.Text = value; }
        }
        public string CboGridViewSelect
        {
            set { cboGridViewSelect.SelectedItem = value; }
        }


        public event EventHandler LoadTemplatesClick;
        public event EventHandler OpenDebitorsTmplateClick;
        public event EventHandler OpenRevRepoTmplateClick;
        public event EventHandler OpenCurrRatesTmplateClick;
        public event EventHandler CalculateClick;
        public event EventHandler CheckClick;
        public event FormEventHandler CboGridViewChanged;
        #endregion

        #region Календарь
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            ReportDate = e.Start;
            lblReportDate.Text = ReportDate.ToShortDateString();
            lblReportDate.ForeColor = Color.Black;
            monthCalendar1.Visible = false;
            CheckLoadPermissions();
        }
        private void btnReportDate_MouseClick(object sender, MouseEventArgs e)
        {
            monthCalendar1.Visible = true;
        }
        #endregion

        #region Установка разрешения на загрузку данных через CheckBoxes
        private void chkFXrates_CheckedChanged(object sender, EventArgs e)
        {
            CheckLoadPermissions();
        }
        private void chkREPO_CheckedChanged(object sender, EventArgs e)
        {
            CheckLoadPermissions();
        }
        private void chkDebitors_CheckedChanged(object sender, EventArgs e)
        {
            CheckLoadPermissions();
        }
        private void CheckLoadPermissions()
        {
            if (chkFXrates.Checked && chkDebitors.Checked && chkREPO.Checked && (lblReportDate.Text != "дата не установлена"))
            {
                btnLoadTemplates.Enabled = true;
            }
            else btnLoadTemplates.Enabled = false;
        }

        #endregion

        
    }
}
