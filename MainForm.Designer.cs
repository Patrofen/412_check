namespace _412_check
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnLoadTemplates = new System.Windows.Forms.Button();
            this.groupBoxDebitors = new System.Windows.Forms.GroupBox();
            this.lblDebitorsErrors = new System.Windows.Forms.Label();
            this.btnOpenDebTmpl = new System.Windows.Forms.Button();
            this.lblDebitorsLines = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.lblReportDate = new System.Windows.Forms.Label();
            this.btnReportDate = new System.Windows.Forms.Button();
            this.groupBoxRevRepo = new System.Windows.Forms.GroupBox();
            this.btnOpenRepoTmpl = new System.Windows.Forms.Button();
            this.lblRepoErrors = new System.Windows.Forms.Label();
            this.lblRepoLines = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboGridViewSelect = new System.Windows.Forms.ComboBox();
            this.groupBoxCurrRates = new System.Windows.Forms.GroupBox();
            this.btnOpenCurrRatesTmpl = new System.Windows.Forms.Button();
            this.lblCurrRatesErrors = new System.Windows.Forms.Label();
            this.lblCurrRatesLines = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkDebitors = new System.Windows.Forms.CheckBox();
            this.chkREPO = new System.Windows.Forms.CheckBox();
            this.chkFXrates = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBoxDebitors.SuspendLayout();
            this.groupBoxRevRepo.SuspendLayout();
            this.groupBoxCurrRates.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(374, 43);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(859, 408);
            this.dataGridView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 496);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1245, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnLoadTemplates
            // 
            this.btnLoadTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadTemplates.Enabled = false;
            this.btnLoadTemplates.Location = new System.Drawing.Point(12, 227);
            this.btnLoadTemplates.Name = "btnLoadTemplates";
            this.btnLoadTemplates.Size = new System.Drawing.Size(96, 23);
            this.btnLoadTemplates.TabIndex = 12;
            this.btnLoadTemplates.Text = "Load templates";
            this.btnLoadTemplates.UseVisualStyleBackColor = true;
            this.btnLoadTemplates.Click += new System.EventHandler(this.btnLoadTemplates_Click);
            // 
            // groupBoxDebitors
            // 
            this.groupBoxDebitors.Controls.Add(this.lblDebitorsErrors);
            this.groupBoxDebitors.Controls.Add(this.btnOpenDebTmpl);
            this.groupBoxDebitors.Controls.Add(this.lblDebitorsLines);
            this.groupBoxDebitors.Controls.Add(this.label4);
            this.groupBoxDebitors.Controls.Add(this.label3);
            this.groupBoxDebitors.Location = new System.Drawing.Point(12, 35);
            this.groupBoxDebitors.Name = "groupBoxDebitors";
            this.groupBoxDebitors.Size = new System.Drawing.Size(331, 58);
            this.groupBoxDebitors.TabIndex = 13;
            this.groupBoxDebitors.TabStop = false;
            this.groupBoxDebitors.Text = "Дебиторы";
            // 
            // lblDebitorsErrors
            // 
            this.lblDebitorsErrors.AutoSize = true;
            this.lblDebitorsErrors.Location = new System.Drawing.Point(183, 36);
            this.lblDebitorsErrors.Name = "lblDebitorsErrors";
            this.lblDebitorsErrors.Size = new System.Drawing.Size(76, 13);
            this.lblDebitorsErrors.TabIndex = 3;
            this.lblDebitorsErrors.Text = "не загружено";
            // 
            // btnOpenDebTmpl
            // 
            this.btnOpenDebTmpl.Image = global::_412_check.Properties.Resources.excel_32px_2;
            this.btnOpenDebTmpl.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnOpenDebTmpl.Location = new System.Drawing.Point(281, 12);
            this.btnOpenDebTmpl.Name = "btnOpenDebTmpl";
            this.btnOpenDebTmpl.Size = new System.Drawing.Size(40, 40);
            this.btnOpenDebTmpl.TabIndex = 18;
            this.btnOpenDebTmpl.UseVisualStyleBackColor = true;
            this.btnOpenDebTmpl.Click += new System.EventHandler(this.btnOpenDebTmpl_Click);
            // 
            // lblDebitorsLines
            // 
            this.lblDebitorsLines.AutoSize = true;
            this.lblDebitorsLines.Location = new System.Drawing.Point(183, 16);
            this.lblDebitorsLines.Name = "lblDebitorsLines";
            this.lblDebitorsLines.Size = new System.Drawing.Size(76, 13);
            this.lblDebitorsLines.TabIndex = 2;
            this.lblDebitorsLines.Text = "не загружено";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Количество ошибок загрузки:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Количество загруженных строк:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Отчетная дата:";
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(283, 10);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 15;
            this.monthCalendar1.Visible = false;
            this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            // 
            // lblReportDate
            // 
            this.lblReportDate.AutoSize = true;
            this.lblReportDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblReportDate.ForeColor = System.Drawing.Color.Red;
            this.lblReportDate.Location = new System.Drawing.Point(107, 12);
            this.lblReportDate.Name = "lblReportDate";
            this.lblReportDate.Size = new System.Drawing.Size(131, 13);
            this.lblReportDate.TabIndex = 16;
            this.lblReportDate.Text = "дата не установлена";
            // 
            // btnReportDate
            // 
            this.btnReportDate.Location = new System.Drawing.Point(241, 9);
            this.btnReportDate.Name = "btnReportDate";
            this.btnReportDate.Size = new System.Drawing.Size(27, 20);
            this.btnReportDate.TabIndex = 17;
            this.btnReportDate.Text = "...";
            this.btnReportDate.UseVisualStyleBackColor = true;
            this.btnReportDate.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnReportDate_MouseClick);
            // 
            // groupBoxRevRepo
            // 
            this.groupBoxRevRepo.Controls.Add(this.btnOpenRepoTmpl);
            this.groupBoxRevRepo.Controls.Add(this.lblRepoErrors);
            this.groupBoxRevRepo.Controls.Add(this.lblRepoLines);
            this.groupBoxRevRepo.Controls.Add(this.label6);
            this.groupBoxRevRepo.Controls.Add(this.label7);
            this.groupBoxRevRepo.Location = new System.Drawing.Point(12, 99);
            this.groupBoxRevRepo.Name = "groupBoxRevRepo";
            this.groupBoxRevRepo.Size = new System.Drawing.Size(331, 58);
            this.groupBoxRevRepo.TabIndex = 14;
            this.groupBoxRevRepo.TabStop = false;
            this.groupBoxRevRepo.Text = "РЕПО";
            // 
            // btnOpenRepoTmpl
            // 
            this.btnOpenRepoTmpl.Image = global::_412_check.Properties.Resources.excel_32px_2;
            this.btnOpenRepoTmpl.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnOpenRepoTmpl.Location = new System.Drawing.Point(281, 12);
            this.btnOpenRepoTmpl.Name = "btnOpenRepoTmpl";
            this.btnOpenRepoTmpl.Size = new System.Drawing.Size(40, 40);
            this.btnOpenRepoTmpl.TabIndex = 19;
            this.btnOpenRepoTmpl.UseVisualStyleBackColor = true;
            this.btnOpenRepoTmpl.Click += new System.EventHandler(this.btnOpenRepoTmpl_Click);
            // 
            // lblRepoErrors
            // 
            this.lblRepoErrors.AutoSize = true;
            this.lblRepoErrors.Location = new System.Drawing.Point(183, 36);
            this.lblRepoErrors.Name = "lblRepoErrors";
            this.lblRepoErrors.Size = new System.Drawing.Size(76, 13);
            this.lblRepoErrors.TabIndex = 3;
            this.lblRepoErrors.Text = "не загружено";
            // 
            // lblRepoLines
            // 
            this.lblRepoLines.AutoSize = true;
            this.lblRepoLines.Location = new System.Drawing.Point(183, 16);
            this.lblRepoLines.Name = "lblRepoLines";
            this.lblRepoLines.Size = new System.Drawing.Size(76, 13);
            this.lblRepoLines.TabIndex = 2;
            this.lblRepoLines.Text = "не загружено";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Количество ошибок загрузки:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(171, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Количество загруженных строк:";
            // 
            // cboGridViewSelect
            // 
            this.cboGridViewSelect.FormattingEnabled = true;
            this.cboGridViewSelect.Items.AddRange(new object[] {
            "Проверка формы",
            "Дебиторы",
            "Кредиторы",
            "РЕПО",
            "Курсы валют"});
            this.cboGridViewSelect.Location = new System.Drawing.Point(374, 12);
            this.cboGridViewSelect.Name = "cboGridViewSelect";
            this.cboGridViewSelect.Size = new System.Drawing.Size(153, 21);
            this.cboGridViewSelect.TabIndex = 18;
            this.cboGridViewSelect.SelectedIndexChanged += new System.EventHandler(this.cboGridViewSelect_SelectedIndexChanged);
            // 
            // groupBoxCurrRates
            // 
            this.groupBoxCurrRates.Controls.Add(this.btnOpenCurrRatesTmpl);
            this.groupBoxCurrRates.Controls.Add(this.lblCurrRatesErrors);
            this.groupBoxCurrRates.Controls.Add(this.lblCurrRatesLines);
            this.groupBoxCurrRates.Controls.Add(this.label8);
            this.groupBoxCurrRates.Controls.Add(this.label9);
            this.groupBoxCurrRates.Location = new System.Drawing.Point(12, 163);
            this.groupBoxCurrRates.Name = "groupBoxCurrRates";
            this.groupBoxCurrRates.Size = new System.Drawing.Size(331, 58);
            this.groupBoxCurrRates.TabIndex = 19;
            this.groupBoxCurrRates.TabStop = false;
            this.groupBoxCurrRates.Text = "Курсы валют";
            // 
            // btnOpenCurrRatesTmpl
            // 
            this.btnOpenCurrRatesTmpl.Image = global::_412_check.Properties.Resources.excel_32px_2;
            this.btnOpenCurrRatesTmpl.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnOpenCurrRatesTmpl.Location = new System.Drawing.Point(281, 12);
            this.btnOpenCurrRatesTmpl.Name = "btnOpenCurrRatesTmpl";
            this.btnOpenCurrRatesTmpl.Size = new System.Drawing.Size(40, 40);
            this.btnOpenCurrRatesTmpl.TabIndex = 19;
            this.btnOpenCurrRatesTmpl.UseVisualStyleBackColor = true;
            this.btnOpenCurrRatesTmpl.Click += new System.EventHandler(this.btnOpenCurrRatesTmpl_Click);
            // 
            // lblCurrRatesErrors
            // 
            this.lblCurrRatesErrors.AutoSize = true;
            this.lblCurrRatesErrors.Location = new System.Drawing.Point(183, 36);
            this.lblCurrRatesErrors.Name = "lblCurrRatesErrors";
            this.lblCurrRatesErrors.Size = new System.Drawing.Size(76, 13);
            this.lblCurrRatesErrors.TabIndex = 3;
            this.lblCurrRatesErrors.Text = "не загружено";
            // 
            // lblCurrRatesLines
            // 
            this.lblCurrRatesLines.AutoSize = true;
            this.lblCurrRatesLines.Location = new System.Drawing.Point(183, 16);
            this.lblCurrRatesLines.Name = "lblCurrRatesLines";
            this.lblCurrRatesLines.Size = new System.Drawing.Size(76, 13);
            this.lblCurrRatesLines.TabIndex = 2;
            this.lblCurrRatesLines.Text = "не загружено";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(159, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Количество ошибок загрузки:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(171, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Количество загруженных строк:";
            // 
            // chkDebitors
            // 
            this.chkDebitors.AutoSize = true;
            this.chkDebitors.Location = new System.Drawing.Point(349, 60);
            this.chkDebitors.Name = "chkDebitors";
            this.chkDebitors.Size = new System.Drawing.Size(15, 14);
            this.chkDebitors.TabIndex = 20;
            this.chkDebitors.UseVisualStyleBackColor = true;
            this.chkDebitors.CheckedChanged += new System.EventHandler(this.chkDebitors_CheckedChanged);
            // 
            // chkREPO
            // 
            this.chkREPO.AutoSize = true;
            this.chkREPO.Location = new System.Drawing.Point(349, 125);
            this.chkREPO.Name = "chkREPO";
            this.chkREPO.Size = new System.Drawing.Size(15, 14);
            this.chkREPO.TabIndex = 21;
            this.chkREPO.UseVisualStyleBackColor = true;
            this.chkREPO.CheckedChanged += new System.EventHandler(this.chkREPO_CheckedChanged);
            // 
            // chkFXrates
            // 
            this.chkFXrates.AutoSize = true;
            this.chkFXrates.Location = new System.Drawing.Point(349, 189);
            this.chkFXrates.Name = "chkFXrates";
            this.chkFXrates.Size = new System.Drawing.Size(15, 14);
            this.chkFXrates.TabIndex = 22;
            this.chkFXrates.UseVisualStyleBackColor = true;
            this.chkFXrates.CheckedChanged += new System.EventHandler(this.chkFXrates_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 518);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.chkFXrates);
            this.Controls.Add(this.chkREPO);
            this.Controls.Add(this.chkDebitors);
            this.Controls.Add(this.groupBoxCurrRates);
            this.Controls.Add(this.cboGridViewSelect);
            this.Controls.Add(this.groupBoxRevRepo);
            this.Controls.Add(this.btnReportDate);
            this.Controls.Add(this.lblReportDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBoxDebitors);
            this.Controls.Add(this.btnLoadTemplates);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MainForm";
            this.Text = "412_check";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBoxDebitors.ResumeLayout(false);
            this.groupBoxDebitors.PerformLayout();
            this.groupBoxRevRepo.ResumeLayout(false);
            this.groupBoxRevRepo.PerformLayout();
            this.groupBoxCurrRates.ResumeLayout(false);
            this.groupBoxCurrRates.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnLoadTemplates;
        private System.Windows.Forms.GroupBox groupBoxDebitors;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDebitorsErrors;
        private System.Windows.Forms.Label lblDebitorsLines;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Label lblReportDate;
        private System.Windows.Forms.Button btnReportDate;
        private System.Windows.Forms.Button btnOpenDebTmpl;
        private System.Windows.Forms.GroupBox groupBoxRevRepo;
        private System.Windows.Forms.Label lblRepoErrors;
        private System.Windows.Forms.Label lblRepoLines;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnOpenRepoTmpl;
        private System.Windows.Forms.ComboBox cboGridViewSelect;
        private System.Windows.Forms.GroupBox groupBoxCurrRates;
        private System.Windows.Forms.Button btnOpenCurrRatesTmpl;
        private System.Windows.Forms.Label lblCurrRatesErrors;
        private System.Windows.Forms.Label lblCurrRatesLines;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkDebitors;
        private System.Windows.Forms.CheckBox chkREPO;
        private System.Windows.Forms.CheckBox chkFXrates;
    }
}

