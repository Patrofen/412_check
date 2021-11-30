using _412.Messages;
//using Microsoft.Office.Interop.Excel;
using _412_check.BL;
using System;
using System.Data;
using System.IO;

namespace _412_check
{
    partial class MainPresenter
    {
        private readonly IMainForm _view;
        private readonly IBusinessLogic _bsnsLogic;
        private readonly IMessageService _messageService;
        private System.Data.DataTableCollection xlsSheetsCollection;
        private int totalDebitorsErrors;
        private int totalRepoErrors;
        private int totalCurrRatesErrors;

        //Справочники
        private System.Data.DataTable tbl_currCatalog;
        private System.Data.DataTable tbl_CptyCatalog;
        //Основные данные
        private System.Data.DataTable tbl_debitorsTemplate;
        private System.Data.DataTable tbl_debitorsSystem = new System.Data.DataTable("debitorsSystem");
        private System.Data.DataTable tbl_commonRepoTemplate;
        private System.Data.DataTable tbl_currRatesTemplate;
        private System.Data.DataTable tbl_currRatesSystem = new System.Data.DataTable("currRatesSystem");
        //private System.Data.DataTable tbl_reverseRepoSystem = new System.Data.DataTable("reverseRepoSystem");
        private System.Data.DataTable tbl_commonRepoSystem = new System.Data.DataTable("commonRepoSystem");


        //Конструктор
        public MainPresenter(IMainForm view, IBusinessLogic bsnsLogic, IMessageService service)
        {
            _view = view;
            _bsnsLogic = bsnsLogic;
            _messageService = service;
            _view.OpenDebitorsTmplateClick += _view_OpenDebitorsTmplateClick;
            _view.OpenRevRepoTmplateClick += _view_OpenRevRepoTmplateClick;
            _view.LoadTemplatesClick += _view_LoadTemplatesClick;
            _view.OpenCurrRatesTmplateClick += _view_OpenCurrRatesTmplateClick;
            _view.CboGridViewChanged += _view_CboGridViewChanged;
            LoadDirectories();
        }

        

        #region Обработчики нажатия кнопок
        //Открыть шаблон Дебиторская_задолженность.xlsx
        private void _view_OpenDebitorsTmplateClick(object sender, EventArgs e)
        {
            FileInfo xlFile_debitTmlp = Utils.GetFileInfo("Шаблоны", "Дебиторская_задолженность.xlsx");
            OpenUsingOfficeInterop(xlFile_debitTmlp);
        }
        //Открыть шаблон РЕПО.xlsx
        private void _view_OpenRevRepoTmplateClick(object sender, EventArgs e)
        {
            FileInfo xlFile_RepoTmlp = Utils.GetFileInfo("Шаблоны", "РЕПО.xlsx");
            OpenUsingOfficeInterop(xlFile_RepoTmlp);
        }
        //Открыть шаблон Курсы_валют.xlsx
        private void _view_OpenCurrRatesTmplateClick(object sender, EventArgs e)
        {
            FileInfo xlFile_currRatesTmlp = Utils.GetFileInfo("Шаблоны", "Курсы_валют.xlsx");
            OpenUsingOfficeInterop(xlFile_currRatesTmlp);
        }

        //Загрузка информации из шаблонов в формате Excel
        private void _view_LoadTemplatesClick(object sender, EventArgs e)
        {
            try
            {
                //Из файла Курсы_валют.xlsx
                FileInfo xlFile_CurrRates_templ = Utils.GetFileInfo("Шаблоны", "Курсы_валют.xlsx");
                tbl_currRatesTemplate = OpenUsingExcelDataReader(xlFile_CurrRates_templ);
                if (tbl_currRatesTemplate != null)
                {
                    _view.TotalCurrRatesLines = tbl_currRatesTemplate.Rows.Count.ToString();
                    CheckAndWriteCurrRates(tbl_currRatesTemplate);
                }
                else
                {
                    _view.TotalCurrRatesLines = "ошибка загрузки";
                    _view.TotalCurrRatesErrors = "ошибка загрузки";
                }

                //Из файла Дебиторская_задолженность.xlsx
                FileInfo xlFile_debitors_templ = Utils.GetFileInfo("Шаблоны", "Дебиторская_задолженность.xlsx");
                tbl_debitorsTemplate = OpenUsingExcelDataReader(xlFile_debitors_templ);
                if (tbl_debitorsTemplate != null)
                {
                    _view.TotalDebitorsLines = tbl_debitorsTemplate.Rows.Count.ToString();
                    CheckAndWriteDebitors(tbl_debitorsTemplate);
                }
                else
                {
                    _view.TotalDebitorsLines = "ошибка загрузки";
                    _view.TotalDebitorsErrors = "ошибка загрузки";
                }

                //Из файла РЕПО.xlsx
                FileInfo xlFile_Repo_templ = Utils.GetFileInfo("Шаблоны", "РЕПО.xlsx");
                tbl_commonRepoTemplate = OpenUsingExcelDataReader(xlFile_Repo_templ);
                if (tbl_commonRepoTemplate != null)
                {
                    _view.TotalRepoLines = tbl_commonRepoTemplate.Rows.Count.ToString();
                    CheckAndWriteREPO(tbl_commonRepoTemplate);
                }
                else
                {
                    _view.TotalRepoLines = "ошибка загрузки";
                    _view.TotalRepoErrors = "ошибка загрузки";
                }

                
                //-------
                _messageService.ShowMessage("Загрузка из шаблонов завершена");
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (Exception ex)
            {
                _messageService.ShowError(ex.ToString());
            }
        }
        //Выбор отображаемой информации в DataGridView
        private void _view_CboGridViewChanged(object sender, FormEventArgs e)
        {
            switch (e.msg)
            {
                case "Дебиторы":
                    _view.DataGridView = tbl_debitorsSystem;
                    break;
                case "РЕПО":
                    _view.DataGridView = tbl_commonRepoSystem;
                    break;
                case "Курсы валют":
                    _view.DataGridView = tbl_currRatesSystem;
                    break;
            }
        }

        #endregion

        //Загрузка справочников
        private void LoadDirectories()
        {
            //Загрузка справочника валют
            FileInfo xlFile_curr_codes = Utils.GetFileInfo("Справочники", "Код_валюты.xlsx");
            tbl_currCatalog = OpenUsingExcelDataReader(xlFile_curr_codes);
            //Загрузка справочника контрагентов
            FileInfo xlFile_cpty = Utils.GetFileInfo("Справочники", "Контрагенты.xlsx");
            tbl_CptyCatalog = OpenUsingExcelDataReader(xlFile_cpty);
        }

        #region Взаимодействие с Excel
        //Из файла Excel в System.Data.DataTable, используя ExcelDataReader
        private System.Data.DataTable OpenUsingExcelDataReader(FileInfo filePath)
        {

            bool isExist = _bsnsLogic.IsExist(filePath.FullName);
            if (!isExist)
            {
                _messageService.ShowExclmation("Файл " + filePath.Name + "не существует");
            }
            xlsSheetsCollection = _bsnsLogic.ExcelToDataTable(filePath);
            try
            {
                System.Data.DataTable result = xlsSheetsCollection[0];
                return result;
            }
            catch (NullReferenceException)
            {
                _messageService.ShowError("Закройте файл " + filePath.Name + "\n\nи повторите загрузку");
                return null;
            }
            catch (Exception ex)
            {
                _messageService.ShowError(ex.Message);
                return null;
            }
        }

        //Открыть Excel файл, используя Microsoft.Office.Interop.Excel
        private void OpenUsingOfficeInterop(FileInfo filePath)
        {
            try
            {
                bool isExist = _bsnsLogic.IsExist(filePath.FullName);
                if (!isExist)
                {
                    _messageService.ShowError("Файл " + filePath.Name + " не существует");
                }
                _bsnsLogic.OpenXlUsingInterop(filePath);
            }
            catch (Exception ex)
            {
                _messageService.ShowError(ex.Message);
            }
        }
        #endregion



        //Проверка данных в шаблоне "Дебиторская_задолженность.xlsx", перенос в системную таблицу tbl_debitorsSystem,
        //выгрузка в файл Debitors_clear.xlsx
        private void CheckAndWriteDebitors(System.Data.DataTable debitors_template)
        {
            totalDebitorsErrors = 0;
            _bsnsLogic.ResetSystemTable(tbl_debitorsSystem);

            foreach (DataRow templ_row in debitors_template.Rows)
            {
                DataRow sys_row = tbl_debitorsSystem.NewRow();

                string debitorID = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Код", "Идентификатор", out int errorsID);
                sys_row["Идентификатор требования к дебитору"] = debitorID;
                totalDebitorsErrors += errorsID;

                string debitorName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Код", "Наименование", out int errorsName);
                sys_row["Наименование дебитора"] = debitorName;
                totalDebitorsErrors += errorsName;

                string currCode = _bsnsLogic.FindCorrespondence(templ_row[2].ToString(), tbl_currCatalog, "Букв. код", "Код", out int errorsCurr);
                sys_row["Код валюты"] = currCode;
                totalDebitorsErrors += errorsCurr;

                sys_row["Объем требования в единицах валюты"] = templ_row[3];
                sys_row["Объем требования в рублях"] = templ_row[4];
                sys_row["Тип требования"] = "дебиторская задолженность";

                switch (templ_row[5])
                {
                    case DateTime maturity:
                        sys_row["Срок погашения"] = MaturutyFromDateToText(maturity);
                        break;
                    case string maturity:
                        if ((maturity == "без срока") |
                            (maturity == "до 1 месяца") |
                            (maturity == "до востребования") |
                            (maturity == "от 1 до 3 месяцев") |
                            (maturity == "от 3 до 6 месяцев") |
                            (maturity == "от 6 месяцев до 1 года") |
                            (maturity == "свыше 1 года"))
                        {
                            sys_row["Срок погашения"] = maturity;
                        }

                        else if (DateTime.TryParse(maturity, out DateTime result))
                        {
                            sys_row["Срок погашения"] = MaturutyFromDateToText(result);
                        }
                        else
                        {
                            sys_row["Срок погашения"] = "error";
                            totalDebitorsErrors += 1;
                        }
                        break;
                }
                tbl_debitorsSystem.Rows.Add(sys_row);
            }
            FileInfo xlFile_debitors_clear = Utils.GetFileInfo("Системные", "Debitors_clear.xlsx");
            _bsnsLogic.DataTableToExcel(tbl_debitorsSystem, xlFile_debitors_clear);
            _view.TotalDebitorsErrors = totalDebitorsErrors.ToString();
        }


        //Проверка данных в шаблоне "РЕПО.xlsx", перенос в системные таблицы tbl_RepoSystem и tbl_reverseRepoSystem,
        //выгрузка в файл REPO_clear.xlsx
        private void CheckAndWriteREPO(System.Data.DataTable REPO_template)
        {
            totalRepoErrors = 0;
            _bsnsLogic.ResetSystemTable(tbl_commonRepoSystem);

            foreach (DataRow templ_row in REPO_template.Rows)
            {
                DataRow sys_row = tbl_commonRepoSystem.NewRow();
                sys_row["Идентификатор требования к дебитору"] = templ_row[0];
                
                string debitorName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Идентификатор", "Наименование", out int errorsName);
                sys_row["Наименование дебитора"] = debitorName;
                totalRepoErrors += errorsName;

                string currCode = _bsnsLogic.FindCorrespondence(templ_row[2].ToString(), tbl_currCatalog, "Букв. код", "Код", out int errorsCurr);
                sys_row["Код валюты"] = currCode;
                totalRepoErrors += errorsCurr;

                
                string curr = templ_row[2].ToString();
                bool is_amountInRUB = double.TryParse(templ_row[4].ToString(), out double amountInRUB);
                bool is_AccInt = double.TryParse(templ_row[3].ToString(), out double AccInt);
                double FxRateDouble = _bsnsLogic.FindDouble(curr, tbl_currRatesSystem, "Букв. код", "Курс", out int errorsFX);

                if (curr == "RUB")
                {
                    if (is_amountInRUB && is_AccInt)
                    {
                        if (templ_row[5].ToString() == "Buy stock ")
                        {
                            sys_row["Объем требования в рублях"] = Math.Round(amountInRUB + AccInt, 2);
                        }
                        else
                        {
                            sys_row["Объем требования в рублях"] = Math.Round(amountInRUB + (-AccInt), 2);
                        }
                    }
                    else
                    {
                        sys_row["Объем требования в рублях"] = 1111111111;
                        totalRepoErrors += 1;
                    }
                }
                else
                {
                    if (is_amountInRUB && is_AccInt && (FxRateDouble != 0))
                    {
                        if (templ_row[5].ToString() == "Buy stock ")
                        {
                            sys_row["Объем требования в рублях"] = Math.Round(amountInRUB + AccInt, 2);
                            sys_row["Объем требования в единицах валюты"] = Math.Round((amountInRUB + AccInt) / FxRateDouble, 2);
                        }
                        else
                        {
                            sys_row["Объем требования в рублях"] = Math.Round(amountInRUB + (-AccInt), 2);
                            sys_row["Объем требования в единицах валюты"] = Math.Round((amountInRUB + (-AccInt)) / FxRateDouble, 2);
                        }
                    }
                    
                    else
                    {
                        sys_row["Объем требования в единицах валюты"] = 1111111111;
                        totalRepoErrors += 1;
                    }
                }
                


                //string curr = templ_row[2].ToString();
                //if (curr != "RUB")
                //{
                //    string FxRateString = _bsnsLogic.FindCorrespondence(curr,tbl_currRatesSystem, "Букв. код", "Курс", out int errorsFX);
                //    bool is_summRUB = double.TryParse(templ_row[4].ToString(), out double summRUB);

                //    bool is_FxRateDouble = double.TryParse(FxRateString, out double FxRateDouble);
                //    if (is_FxRateDouble && is_summRUB)
                //    {
                //        double RubSumm = sys_row["Объем требования в рублях"]
                //        sys_row["Объем требования в единицах валюты"] = FxRateDouble;
                //    }
                //    else
                //    {
                //        sys_row["Объем требования в единицах валюты"] = "error";
                //        totalCurrRatesErrors += errorsFX;
                //    }
                    
                //}
                //else
                //{
                //    sys_row["Объем требования в единицах валюты"] = sys_row["Объем требования в рублях"];
                //}
                


                //sys_row["Тип требования"] = "дебиторская задолженность";

                //switch (templ_row[5])
                //{
                //    case DateTime maturity:
                //        sys_row["Срок погашения"] = MaturutyFromDateToText(maturity);
                //        break;
                //    case string maturity:
                //        if ((maturity == "без срока") |
                //            (maturity == "до 1 месяца") |
                //            (maturity == "до востребования") |
                //            (maturity == "от 1 до 3 месяцев") |
                //            (maturity == "от 3 до 6 месяцев") |
                //            (maturity == "от 6 месяцев до 1 года") |
                //            (maturity == "свыше 1 года"))
                //        {
                //            sys_row["Срок погашения"] = maturity;
                //        }

                //        else if (DateTime.TryParse(maturity, out DateTime result))
                //        {
                //            sys_row["Срок погашения"] = MaturutyFromDateToText(result);
                //        }
                //        else
                //        {
                //            sys_row["Срок погашения"] = "error";
                //            totalRepoErrors += 1;
                //        }
                //        break;
                //}
                tbl_commonRepoSystem.Rows.Add(sys_row);
            }
            FileInfo xlFile_Repo_clear = Utils.GetFileInfo("Системные", "REPO_clear.xlsx");
            _bsnsLogic.DataTableToExcel(tbl_commonRepoSystem, xlFile_Repo_clear);
            _view.TotalRepoErrors = totalRepoErrors.ToString();
        }

        //Проверка данных в шаблоне "Курсы_валют.xlsx" и перенос в системную таблицу tbl_currRatesSystem,
        //выгрузка в файл Curr_rates_system.xlsx
        private void CheckAndWriteCurrRates (System.Data.DataTable CurrRates_template)
        {
            totalCurrRatesErrors = 0;
            _bsnsLogic.ResetSystemTable(tbl_currRatesSystem);

            foreach (DataRow templ_row in CurrRates_template.Rows)
            {
                DataRow sys_row = tbl_currRatesSystem.NewRow();

                bool is_NumCode = int.TryParse(templ_row[0].ToString(), out int numCode);
                if (is_NumCode)
                {
                    sys_row["Цифр. код"] = numCode;
                }
                else
                {
                    sys_row["Цифр. код"] = "error";
                    totalCurrRatesErrors += 1;
                }

                sys_row["Букв. код"] = templ_row[1].ToString();

                bool is_Units = int.TryParse(templ_row[2].ToString(), out int units);
                if (is_Units)
                {
                    sys_row["Единиц"] = units;
                }
                else
                {
                    sys_row["Единиц"] = "error";
                    totalCurrRatesErrors += 1;
                }

                sys_row["Валюта"] = templ_row[3].ToString();

                bool is_FXrate = double.TryParse(templ_row[4].ToString(), out double FXrate);
                if (is_Units)
                {
                    sys_row["Курс"] = FXrate;
                }
                else
                {
                    sys_row["Курс"] = "error";
                    totalCurrRatesErrors += 1;
                }
                tbl_currRatesSystem.Rows.Add(sys_row);
            }
            FileInfo xlFile_CurrRatesSustem = Utils.GetFileInfo("Системные", "Curr_rates_system.xlsx");
            _bsnsLogic.DataTableToExcel(tbl_currRatesSystem, xlFile_CurrRatesSustem);
            _view.TotalCurrRatesErrors = totalCurrRatesErrors.ToString();
        }

        //Срок погашения из даты в текст
        public string MaturutyFromDateToText(DateTime date)
        {
            TimeSpan delta = date.Subtract(_view.ReportDate);
            TimeSpan daysInNext_1_Month = _view.ReportDate.AddMonths(1) - _view.ReportDate;
            TimeSpan daysInNext_3_Months = _view.ReportDate.AddMonths(3) - _view.ReportDate;
            TimeSpan daysInNext_6_Months = _view.ReportDate.AddMonths(6) - _view.ReportDate;
            TimeSpan daysInNext_12_Months = _view.ReportDate.AddMonths(12) - _view.ReportDate;
            //просроченная задолженность
            if (delta.Days < 0)
            {
                return "просрочена";
            }
            //до 1 месяца
            else if (delta.Days > 0 && delta.Days <= daysInNext_1_Month.Days)
            {
                return "до 1 месяца";
            }
            //от 1 до 3 месяцев
            else if (delta.Days > daysInNext_1_Month.Days && delta.Days <= daysInNext_3_Months.Days)
            {
                return "от 1 до 3 месяцев";
            }
            //от 3 до 6 месяцев
            else if (delta.Days > daysInNext_3_Months.Days && delta.Days <= daysInNext_6_Months.Days)
            {
                return "от 3 до 6 месяцев";
            }
            //от 6 месяцев до 1 года
            else if (delta.Days > daysInNext_6_Months.Days && delta.Days <= daysInNext_12_Months.Days)
            {
                return "от 6 месяцев до 1 года";
            }
            //свыше 1 года
            else if (delta.Days > daysInNext_12_Months.Days)
            {
                return "свыше 1 года";
            }
            else return "error";
        }
    }
}