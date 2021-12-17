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
        string TotalCreditorsLines { set; }
        string TotalCreditorsErrors { set; }
        string TotalShortsLines { set; }
        string TotalShortsErrors { set; }
        string TotalRepoLines { set; }
        string TotalRepoErrors { set; }
        string TotalCurrRatesLines { set; }
        string TotalCurrRatesErrors { set; }
        string CboGridViewSelect { set; }


        event EventHandler LoadTemplatesClick;
        event EventHandler OpenDebitorsTmplateClick;
        event EventHandler OpenCreditorsTmplateClick;
        event EventHandler OpenShortsTmplateClick;
        event EventHandler OpenRepoTmplateClick;
        event EventHandler OpenCurrRatesTmplateClick;
        event EventHandler Calculate1sectClick;
        event EventHandler Calculate2sectClick;
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
            btnCalculate1sect.Enabled = true;
            btnCalculate2sect.Enabled = true;
        }
        private void btnOpenDebTmpl_Click(object sender, EventArgs e)
        {
            if (OpenDebitorsTmplateClick != null) OpenDebitorsTmplateClick(this, EventArgs.Empty);
        }
        private void btnOpenCredTmpl_Click(object sender, EventArgs e)
        {
            if (OpenCreditorsTmplateClick != null) OpenCreditorsTmplateClick(this, EventArgs.Empty);
        }
        private void btnOpenShortsTempl_Click(object sender, EventArgs e)
        {
            if (OpenShortsTmplateClick != null) OpenShortsTmplateClick(this, EventArgs.Empty);
        }
        private void btnOpenRepoTmpl_Click(object sender, EventArgs e)
        {
            if (OpenRepoTmplateClick != null) OpenRepoTmplateClick(this, EventArgs.Empty);
        }
        private void btnOpenCurrRatesTmpl_Click(object sender, EventArgs e)
        {
            if (OpenCurrRatesTmplateClick != null) OpenCurrRatesTmplateClick(this, EventArgs.Empty);
        }
        private void btnCalculate1sect_Click(object sender, EventArgs e)
        {
            if (Calculate1sectClick != null) Calculate1sectClick(this, EventArgs.Empty);
            //btnCheck.Enabled = true;
        }
        private void btnCalculate2sect_Click(object sender, EventArgs e)
        {
            if (Calculate2sectClick != null) Calculate2sectClick(this, EventArgs.Empty);
            //btnCheck.Enabled = true;
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
        //Количество строк Кредиторской задолженности
        public string TotalCreditorsLines
        {
            set { lblCreditorsLines.Text = value; }
        }
        //Количество ошибок загрузки Кредиторской задолженности
        public string TotalCreditorsErrors
        {
            set { lblCreditorsErrors.Text = value; }
        }
        //Количество строк Коротких позиций
        public string TotalShortsLines
        {
            set { lblShortsLines.Text = value; }
        }
        //Количество ошибок загрузки Коротких позиций
        public string TotalShortsErrors
        {
            set { lblShortsErrors.Text = value; }
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
        public event EventHandler OpenCreditorsTmplateClick;
        public event EventHandler OpenShortsTmplateClick;
        public event EventHandler OpenRepoTmplateClick;
        public event EventHandler OpenCurrRatesTmplateClick;
        public event EventHandler Calculate1sectClick;
        public event EventHandler Calculate2sectClick;
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
        private void chkCreditors_CheckedChanged(object sender, EventArgs e)
        {
            CheckLoadPermissions();
        }
        private void chkShorts_CheckedChanged(object sender, EventArgs e)
        {
            CheckLoadPermissions();
        }
        private void CheckLoadPermissions()
        {
            if (chkDebitors.Checked && chkCreditors.Checked && chkShorts.Checked && chkREPO.Checked && chkFXrates.Checked && (lblReportDate.Text != "дата не установлена"))
            {
                btnLoadTemplates.Enabled = true;
            }
            else btnLoadTemplates.Enabled = false;
        }
        #endregion

        
    }
}
