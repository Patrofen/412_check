using _412.Messages;
//using Microsoft.Office.Interop.Excel;
using _412_check.BL;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace _412_check
{
    partial class MainPresenter
    {
        private readonly IMainForm _view;
        private readonly IBusinessLogic _bsnsLogic;
        private readonly IMessageService _messageService;
        private System.Data.DataTableCollection xlsSheetsCollection;
        private int totalDebitorsErrors;
        private int totalCreditorsErrors;
        private int totalShortsErrors;
        private int totalRepoErrors;
        private int totalCurrRatesErrors;

        //Справочники
        private System.Data.DataTable tbl_currCatalog;
        private System.Data.DataTable tbl_CptyCatalog;
        //Основные данные
        private System.Data.DataTable tbl_debitorsTemplate; //Шаблон для загрузки превичных данных по всей дебиторке
        private System.Data.DataTable tbl_debitorsSystem = new System.Data.DataTable("debitorsSystem"); //Дебиторка - обработанные данные
        private System.Data.DataTable tbl_creditorsTemplate; //Шаблон для загрузки превичных данных по всей кредиторской задолж.
        private System.Data.DataTable tbl_creditorsSystem = new System.Data.DataTable("creditorsSystem"); //Кред. задолж. - обработанные данные
        private System.Data.DataTable tbl_shortsTemplate; //Шаблон для загрузки превичных данных по коротким позициям
        private System.Data.DataTable tbl_shortsSystem = new System.Data.DataTable("shortsSystem"); //Короткие позиции - обработанные данные
        private System.Data.DataTable tbl_commonRepoTemplate; //Шаблон для загрузки первичных данных по всем сделкам РЕПО
        private System.Data.DataTable tbl_currRatesTemplate; //Шаблон для загрузки первичных данных по курсам валют
        private System.Data.DataTable tbl_currRatesSystem = new System.Data.DataTable("currRatesSystem"); //Курсы валют - обработанные данные
        private System.Data.DataTable tbl_commonRepoSystem = new System.Data.DataTable("commonRepoSystem"); //РЕПО - обработанные данные
        private System.Data.DataTable tbl_CommonDebitSystem = new System.Data.DataTable("commonDebitSystem"); //Общая для всех данных 1-го раздела
        private System.Data.DataTable tbl_DebitSystem = new System.Data.DataTable("debitSystem"); //Выборка данных >1% для 1-го раздела (детализированная)
        private System.Data.DataTable tbl_GroupedDebitSystem = new System.Data.DataTable("groupedDebitSystem"); //Рассчитанный 1-й раздел
        private System.Data.DataTable tbl_CommonCreditSystem = new System.Data.DataTable("commonCreditSystem"); //Общая для всех данных 2-го раздела
        private System.Data.DataTable tbl_CreditSystem = new System.Data.DataTable("creditSystem"); //Выборка данных >1% для 2-го раздела (детализированная)
        private System.Data.DataTable tbl_GroupedCreditSystem = new System.Data.DataTable("groupedCreditSystem"); //Рассчитанный 2-й раздел


        //Конструктор
        public MainPresenter(IMainForm view, IBusinessLogic bsnsLogic, IMessageService service)
        {
            _view = view;
            _bsnsLogic = bsnsLogic;
            _messageService = service;
            _view.OpenDebitorsTmplateClick += _view_OpenDebitorsTmplateClick;
            _view.OpenCreditorsTmplateClick += _view_OpenCreditorsTmplateClick;
            _view.OpenShortsTmplateClick += _view_OpenShortsTmplateClick;
            _view.OpenRepoTmplateClick += _view_OpenRepoTmplateClick;
            _view.LoadTemplatesClick += _view_LoadTemplatesClick;
            _view.OpenCurrRatesTmplateClick += _view_OpenCurrRatesTmplateClick;
            _view.Calculate1sectClick += _view_Calculate1sectClick;
            _view.Calculate2sectClick += _view_Calculate2sectClick;
            _view.CheckClick += _view_CheckClick;
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
        //Открыть шаблон Кредиторская_задолженность.xlsx
        private void _view_OpenCreditorsTmplateClick(object sender, EventArgs e)
        {
            FileInfo xlFile_creditTmlp = Utils.GetFileInfo("Шаблоны", "Кредиторская_задолженность.xlsx");
            OpenUsingOfficeInterop(xlFile_creditTmlp);
        }
        //Открыть шаблон Короткие_позиции.xlsx
        private void _view_OpenShortsTmplateClick(object sender, EventArgs e)
        {
            FileInfo xlFile_shortsTmlp = Utils.GetFileInfo("Шаблоны", "Короткие_позиции.xlsx");
            OpenUsingOfficeInterop(xlFile_shortsTmlp);
        }
        //Открыть шаблон РЕПО.xlsx
        private void _view_OpenRepoTmplateClick(object sender, EventArgs e)
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

                //Из файла Кредиторская_задолженность.xlsx
                FileInfo xlFile_creditors_templ = Utils.GetFileInfo("Шаблоны", "Кредиторская_задолженность.xlsx");
                tbl_creditorsTemplate = OpenUsingExcelDataReader(xlFile_creditors_templ);
                if (tbl_creditorsTemplate != null)
                {
                    _view.TotalCreditorsLines = tbl_creditorsTemplate.Rows.Count.ToString();
                    CheckAndWriteCreditors(tbl_creditorsTemplate);
                }
                else
                {
                    _view.TotalCreditorsLines = "ошибка загрузки";
                    _view.TotalCreditorsErrors = "ошибка загрузки";
                }
                
                //Из файла Короткие_позиции.xlsx
                FileInfo xlFile_shorts_templ = Utils.GetFileInfo("Шаблоны", "Короткие_позиции.xlsx");
                tbl_shortsTemplate = OpenUsingExcelDataReader(xlFile_shorts_templ);
                if (tbl_shortsTemplate != null)
                {
                    _view.TotalShortsLines = tbl_shortsTemplate.Rows.Count.ToString();
                    CheckAndWriteShorts(tbl_shortsTemplate);
                }
                else
                {
                    _view.TotalShortsLines = "ошибка загрузки";
                    _view.TotalShortsErrors = "ошибка загрузки";
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
                case "Кредиторы":
                    _view.DataGridView = tbl_creditorsSystem;
                    break;
                case "Короткие позиции":
                    _view.DataGridView = tbl_shortsSystem;
                    break;
                case "РЕПО":
                    _view.DataGridView = tbl_commonRepoSystem;
                    break;
                case "Курсы валют":
                    _view.DataGridView = tbl_currRatesSystem;
                    break;
                case "Раздел 1 сгруппированный":
                    _view.DataGridView = tbl_GroupedDebitSystem;
                    break;
                case "Раздел 1 детализированный":
                    _view.DataGridView = tbl_DebitSystem;
                    break;
                case "Раздел 2 сгруппированный":
                    _view.DataGridView = tbl_GroupedCreditSystem;
                    break;
                case "Раздел 2 детализированный":
                    _view.DataGridView = tbl_CreditSystem;
                    break;
            }
        }

        //Расчет значений показателей 1-го раздела формы
        private void _view_Calculate1sectClick(object sender, EventArgs e)
        {
            _bsnsLogic.ResetSystemTable(tbl_CommonDebitSystem);
            _bsnsLogic.ResetSystemTable(tbl_GroupedDebitSystem);
            _bsnsLogic.ResetSystemTable(tbl_DebitSystem);

            //Копирование всех строк дебиторки в общую таблицу
            foreach (DataRow dr in tbl_debitorsSystem.Rows)
            {
                tbl_CommonDebitSystem.ImportRow(dr);
            }

            //Копирование всех строк обратного РЕПО (buy stock) в общую таблицу
            foreach (DataRow dr in tbl_commonRepoSystem.Rows)
            {
                if (dr["Тип требования или обязательства"].ToString() == "требования по сделкам репо")
                {
                    DataRow row = tbl_CommonDebitSystem.NewRow();
                    row[0] = dr[0];
                    row[1] = dr[1];
                    row[2] = dr[2];
                    row[3] = dr[3];
                    row[4] = dr[4];
                    row[5] = dr[5];
                    row[6] = dr[6];
                    tbl_CommonDebitSystem.Rows.Add(row);
                }
            }
             
            
            double _1percent = 0.01 * tbl_CommonDebitSystem.AsEnumerable().Sum(x => x.Field<double>("Объем требования в рублях"));

            var debitorsFor1Sect = from deb in tbl_CommonDebitSystem.AsEnumerable()
                                   group deb by deb.Field<string>("Наименование дебитора") into groupedDeb
                                   where groupedDeb.Sum(x => x.Field<double>("Объем требования в рублях")) > _1percent
                                   select groupedDeb;

            //Заполнение детализированной информации по первому разделу
            foreach (var group in debitorsFor1Sect)
            {
                foreach (DataRow item in group)
                {
                    DataRow row = tbl_DebitSystem.NewRow();
                    row[0] = item[0];
                    row[1] = item[1];
                    row[2] = item[2];
                    row[3] = item[3];
                    row[4] = item[4];
                    row[5] = item[5];
                    row[6] = item[6];
                    tbl_DebitSystem.Rows.Add(row);
                }
            }
            _view.CboGridViewSelect= "Раздел 1 детализированный";

            //Заполнение сгруппированной информации по первому разделу
            foreach (var _group in debitorsFor1Sect)
            {
                var grByCurr = from deb in _group
                             group deb by deb.Field<string>("Код валюты") into groupedByCurr
                             select groupedByCurr;

                foreach (var item in grByCurr)
                {
                    var grByClType = from deb in item
                                     group deb by deb.Field<string>("Тип требования") into groupedByClType
                                     select groupedByClType;
                    
                    foreach (var item2 in grByClType)
                    {
                        var grByMaturDate = from deb in item2
                                            group deb by deb.Field<string>("Срок погашения") into groupedByMaturDate
                                            select groupedByMaturDate;

                        foreach (var _group2 in grByMaturDate)
                        {
                            DataRow row = tbl_GroupedDebitSystem.NewRow();
                            row[0] = _group2.First().Field<string>("Идентификатор требования к дебитору");
                            row[1] = _group2.First().Field<string>("Наименование дебитора");
                            row[2] = _group2.First().Field<string>("Код валюты");
                            row[4] = Math.Round(_group2.Sum(x => x.Field<double>("Объем требования в рублях")),2);
                            if (row[2].ToString() != "643-RUB")
                            {
                                row[3] = Math.Round(_group2.Sum(x => x.Field<double>("Объем требования в единицах валюты")),2);
                            }
                            else
                            {
                                row[3] = row[4];
                            }
                            row[5] = _group2.First().Field<string>("Тип требования");
                            row[6] = _group2.First().Field<string>("Срок погашения");

                            tbl_GroupedDebitSystem.Rows.Add(row);
                        }
                    }
                }
            }
            _view.CboGridViewSelect = "Раздел 1 сгруппированный";
        }

        //Расчет значений показателей 2-го раздела формы
        private void _view_Calculate2sectClick(object sender, EventArgs e)
        {
            _bsnsLogic.ResetSystemTable(tbl_CommonCreditSystem);
            _bsnsLogic.ResetSystemTable(tbl_GroupedCreditSystem);
            _bsnsLogic.ResetSystemTable(tbl_CreditSystem);

            //Копирование всех строк кредиторской задолженности в общую таблицу
            foreach (DataRow dr in tbl_creditorsSystem.Rows)
            {
                tbl_CommonCreditSystem.ImportRow(dr);
            }

            //Копирование всех строк прямого РЕПО (sell stock) в общую таблицу
            foreach (DataRow dr in tbl_commonRepoSystem.Rows)
            {
                if (dr["Тип требования или обязательства"].ToString() == "обязательства по сделкам репо")
                {
                    DataRow row = tbl_CommonCreditSystem.NewRow();
                    row[0] = dr[0];
                    row[1] = dr[1];
                    row[2] = dr[2];
                    row[3] = dr[3];
                    row[4] = dr[4];
                    row[5] = dr[5];
                    row[6] = dr[6];
                    tbl_CommonCreditSystem.Rows.Add(row);
                }
            }

            //Копирование всех строк короткой позиции в общую таблицу
            foreach (DataRow dr in tbl_shortsSystem.Rows)
            {
                tbl_CommonCreditSystem.ImportRow(dr);
            }

            //_view.DataGridView = tbl_CommonCreditSystem;

            double _1percent = 0.01 * tbl_CommonCreditSystem.AsEnumerable().Sum(x => x.Field<double>("Объем обязательства в рублях"));

            var creditorsFor1Sect = from cred in tbl_CommonCreditSystem.AsEnumerable()
                                    group cred by cred.Field<string>("Наименование кредитора") into groupedCred
                                    where groupedCred.Sum(x => x.Field<double>("Объем обязательства в рублях")) > _1percent
                                    select groupedCred;

            //Заполнение детализированной информации по 2-му разделу
            foreach (var group in creditorsFor1Sect)
            {
                foreach (DataRow item in group)
                {
                    DataRow row = tbl_CreditSystem.NewRow();
                    row[0] = item[0];
                    row[1] = item[1];
                    row[2] = item[2];
                    row[3] = item[3];
                    row[4] = item[4];
                    row[5] = item[5];
                    row[6] = item[6];
                    tbl_CreditSystem.Rows.Add(row);
                }
            }

            //Заполнение сгруппированной информации по 2-му разделу
            foreach (var _group in creditorsFor1Sect)
            {
                var grByCurr = from cred in _group
                               group cred by cred.Field<string>("Код валюты") into groupedByCurr
                               select groupedByCurr;

                foreach (var item in grByCurr)
                {
                    var grByObligType = from cred in item
                                        group cred by cred.Field<string>("Тип обязательства") into groupedByObligType
                                        select groupedByObligType;

                    foreach (var item2 in grByObligType)
                    {
                        var grByMaturDate = from cred in item2
                                            group cred by cred.Field<string>("Срок погашения") into groupedByMaturDate
                                            select groupedByMaturDate;

                        foreach (var _group2 in grByMaturDate)
                        {
                            DataRow row = tbl_GroupedCreditSystem.NewRow();
                            row[0] = _group2.First().Field<string>("Идентификатор обязательства перед кредитором");
                            row[1] = _group2.First().Field<string>("Наименование кредитора");
                            row[2] = _group2.First().Field<string>("Код валюты");
                            row[4] = Math.Round(_group2.Sum(x => x.Field<double>("Объем обязательства в рублях")), 2);
                            if (row[2].ToString() != "643-RUB")
                            {
                                row[3] = Math.Round(_group2.Sum(x => x.Field<double>("Объем обязательства в единицах валюты")), 2);
                            }
                            else
                            {
                                row[3] = row[4];
                            }
                            row[5] = _group2.First().Field<string>("Тип обязательства");
                            row[6] = _group2.First().Field<string>("Срок погашения");

                            tbl_GroupedCreditSystem.Rows.Add(row);
                        }
                    }
                }
            }
            _view.CboGridViewSelect = "Раздел 2 сгруппированный";
        }




        //Сверка расчитанных значений с отчетом 0420402
        private void _view_CheckClick(object sender, EventArgs e)
        {
            _messageService.ShowMessage("Функция еще не реализована");
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

                sys_row["Идентификатор требования к дебитору"] = templ_row[0].ToString();

                string debitorName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Идентификатор", "Наименование", out int errorsName);
                if (debitorName == null)
                {
                    debitorName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Идентификатор", "Полное наименование", out int errorsFullName);
                    totalDebitorsErrors += errorsFullName;
                }
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
                        if (sys_row["Срок погашения"].ToString() == "error") totalDebitorsErrors += 1;
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
                            if (sys_row["Срок погашения"].ToString() == "error") totalDebitorsErrors += 1;
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
            //FileInfo xlFile_debitors_clear = Utils.GetFileInfo("Системные", "Debitors_clear.xlsx");
            //_bsnsLogic.DataTableToExcel(tbl_debitorsSystem, xlFile_debitors_clear);
            _view.TotalDebitorsErrors = totalDebitorsErrors.ToString();
        }



        //Проверка данных в шаблоне "Кредиторская_задолженность.xlsx", перенос в системную таблицу tbl_creditorsSystem,
        private void CheckAndWriteCreditors(System.Data.DataTable creditors_template)
        {
            totalCreditorsErrors = 0;
            _bsnsLogic.ResetSystemTable(tbl_creditorsSystem);

            foreach (DataRow templ_row in creditors_template.Rows)
            {
                DataRow sys_row = tbl_creditorsSystem.NewRow();

                sys_row["Идентификатор обязательства перед кредитором"] = templ_row[0].ToString();

                string creditorName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Идентификатор", "Наименование", out int errorsName);
                if (creditorName == null)
                {
                    creditorName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Идентификатор", "Полное наименование", out int errorsFullName);
                    totalCreditorsErrors += errorsFullName;
                }
                sys_row["Наименование кредитора"] = creditorName;
                totalCreditorsErrors += errorsName;

                string currCode = _bsnsLogic.FindCorrespondence(templ_row[2].ToString(), tbl_currCatalog, "Букв. код", "Код", out int errorsCurr);
                sys_row["Код валюты"] = currCode;
                totalCreditorsErrors += errorsCurr;

                sys_row["Объем обязательства в единицах валюты"] = -(double)templ_row[3];
                sys_row["Объем обязательства в рублях"] = -(double)templ_row[4];
                sys_row["Тип обязательства"] = "кредиторская задолженность";

                switch (templ_row[5])
                {
                    case DateTime maturity:
                        sys_row["Срок погашения"] = MaturutyFromDateToText(maturity);
                        if (sys_row["Срок погашения"].ToString() == "error") totalCreditorsErrors += 1;
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
                            if (sys_row["Срок погашения"].ToString() == "error") totalCreditorsErrors += 1;
                              
                        }
                        else
                        {
                            sys_row["Срок погашения"] = "error";
                            totalCreditorsErrors += 1;
                        }
                        break;
                }
                tbl_creditorsSystem.Rows.Add(sys_row);
            }
            _view.TotalCreditorsErrors = totalCreditorsErrors.ToString();
        }
        
        //Проверка данных в шаблоне "Короткие_позиции.xlsx", перенос в системную таблицу tbl_shortsSystem,
        private void CheckAndWriteShorts(System.Data.DataTable shorts_template)
        {
            totalShortsErrors = 0;
            _bsnsLogic.ResetSystemTable(tbl_shortsSystem);

            foreach (DataRow templ_row in shorts_template.Rows)
            {
                DataRow sys_row = tbl_shortsSystem.NewRow();

                sys_row["Идентификатор обязательства перед кредитором"] = templ_row[0].ToString();

                string ShortCptyName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Идентификатор", "Наименование", out int errorsName);
                if (ShortCptyName == null)
                {
                    ShortCptyName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Идентификатор", "Полное наименование", out int errorsFullName);
                    totalShortsErrors += errorsFullName;
                }
                sys_row["Наименование кредитора"] = ShortCptyName;
                totalShortsErrors += errorsName;

                string currCode = _bsnsLogic.FindCorrespondence(templ_row[2].ToString(), tbl_currCatalog, "Букв. код", "Код", out int errorsCurr);
                sys_row["Код валюты"] = currCode;
                totalShortsErrors += errorsCurr;

                sys_row["Объем обязательства в единицах валюты"] = -(double)templ_row[3];
                sys_row["Объем обязательства в рублях"] = -(double)templ_row[4];
                sys_row["Тип обязательства"] = "иные обязательства, оцениваемые по справедливой стоимости через прибыль или убыток";

                switch (templ_row[5])
                {
                    case DateTime maturity:
                        sys_row["Срок погашения"] = MaturutyFromDateToText(maturity);
                        if (sys_row["Срок погашения"].ToString() == "error") totalShortsErrors += 1;
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
                            if (sys_row["Срок погашения"].ToString() == "error") totalShortsErrors += 1;

                        }
                        else
                        {
                            sys_row["Срок погашения"] = "error";
                            totalShortsErrors += 1;
                        }
                        break;
                }
                tbl_shortsSystem.Rows.Add(sys_row);
            }
            _view.TotalShortsErrors = totalShortsErrors.ToString();
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
                sys_row["Идентификатор контрагента"] = templ_row[0];

                string CptyName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Идентификатор", "Наименование", out int errorsName);
                if (CptyName == null)
                {
                    CptyName = _bsnsLogic.FindCorrespondence(templ_row[0].ToString(), tbl_CptyCatalog, "Идентификатор", "Полное наименование", out int errorsFullName);
                    totalRepoErrors += errorsFullName;
                }
                sys_row["Наименование контрагента"] = CptyName;
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
                            sys_row["Объем требования или обязательства в рублях"] = Math.Round(amountInRUB + AccInt, 2);
                        }
                        else
                        {
                            sys_row["Объем требования или обязательства в рублях"] = Math.Round(amountInRUB + (-AccInt), 2);
                        }
                    }
                    else
                    {
                        sys_row["Объем требования или обязательства в рублях"] = 0;
                        totalRepoErrors += 1;
                    }
                }
                else
                {
                    if (is_amountInRUB && is_AccInt && (FxRateDouble != 0))
                    {
                        if (templ_row[5].ToString() == "Buy stock ")
                        {
                            sys_row["Объем требования или обязательства в рублях"] = Math.Round(amountInRUB + AccInt, 2);
                            sys_row["Объем требования или обязательства в единицах валюты"] = Math.Round((amountInRUB + AccInt) / FxRateDouble, 2);
                        }
                        else
                        {
                            sys_row["Объем требования или обязательства в рублях"] = Math.Round(amountInRUB + (-AccInt), 2);
                            sys_row["Объем требования или обязательства в единицах валюты"] = Math.Round((amountInRUB + (-AccInt)) / FxRateDouble, 2);
                        }
                    }
                    else
                    {
                        sys_row["Объем требования или обязательства в единицах валюты"] = 0;
                        totalRepoErrors += 1;
                    }
                }

                if (templ_row[5].ToString() == "Buy stock ")
                {
                    sys_row["Тип требования или обязательства"] = "требования по сделкам репо";
                }
                else if (templ_row[5].ToString() == "Sell stock")
                {
                    sys_row["Тип требования или обязательства"] = "обязательства по сделкам репо";
                }
                else
                {
                    sys_row["Тип требования или обязательства"] = "error";
                    totalRepoErrors += 1;
                }

                string maturity = templ_row[6].ToString();
                if (DateTime.TryParse(maturity, out DateTime result))
                {
                    sys_row["Срок погашения"] = MaturutyFromDateToText(result);
                    if (sys_row["Срок погашения"].ToString() == "error") totalRepoErrors += 1;
                }
                else
                {
                    sys_row["Срок погашения"] = "error";
                    totalRepoErrors += 1;
                }
                tbl_commonRepoSystem.Rows.Add(sys_row);
            }
            //FileInfo xlFile_Repo_clear = Utils.GetFileInfo("Системные", "REPO_clear.xlsx");
            //_bsnsLogic.DataTableToExcel(tbl_commonRepoSystem, xlFile_Repo_clear);
            _view.TotalRepoErrors = totalRepoErrors.ToString();
        }

        //Проверка данных в шаблоне "Курсы_валют.xlsx" и перенос в системную таблицу tbl_currRatesSystem,
        //выгрузка в файл Curr_rates_system.xlsx
        private void CheckAndWriteCurrRates(System.Data.DataTable CurrRates_template)
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
                    sys_row["Цифр. код"] = 0;
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
                    sys_row["Единиц"] = 0;
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
                    sys_row["Курс"] = 0;
                    totalCurrRatesErrors += 1;
                }
                tbl_currRatesSystem.Rows.Add(sys_row);
            }
            //FileInfo xlFile_CurrRatesSustem = Utils.GetFileInfo("Системные", "Curr_rates_system.xlsx");
            //_bsnsLogic.DataTableToExcel(tbl_currRatesSystem, xlFile_CurrRatesSustem);
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
            if (delta.Days <= 0)
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

//public static class CustomLINQtoDataSetMethods
//{
//    public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
//    {
//        return new ObjectShredder<T>().Shred(source, null, null);
//    }

//    public static DataTable CopyToDataTable<T>(this IEnumerable<T> source,
//                                                DataTable table, LoadOption? options)
//    {
//        return new ObjectShredder<T>().Shred(source, table, options);
//    }
//}