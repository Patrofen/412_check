using System;
using System.IO;
using System.Data;
using _412_check.BL;
using _412.Messages;

namespace _412_check
{
    class MainPresenter
    {
        private readonly IMainForm _view;
        private readonly IModel _model;
        private readonly IMessageService _messageService;
        private DataTableCollection xlsSheetsCollection;
        private DataTable currencies;

        public MainPresenter(IMainForm view, IModel model, IMessageService service)
        {
            _view = view;
            _model = model;
            _messageService = service;
            _view.FileOpenClick += _view_FileOpenClick;
            _view.LoadTemplatesClick += _view_LoadTemplatesClick;
            _view.CboSheetSelectedIndexChanged += _view_CboSheetSelectedIndexChanged;
            //_view.FileSaveClick += _view_FileSaveClick;
            LoadDirectories();
        }

        private void LoadDirectories()
        {
            FileInfo xlFile_currencies = Utils.GetFileInfo("Справочники", "Код_валюты.xlsx");
            currencies = OpenXlFile(xlFile_currencies.FullName);

        }

        private DataTable OpenXlFile(string filePath)
        {
            try
            {
                bool isExist = _model.IsExist(filePath);
                if (!isExist)
                {
                    _messageService.ShowExclmation("Выбранный файл не существует");
                }
                xlsSheetsCollection = _model.GetContent(filePath);
                DataTable result = xlsSheetsCollection[0];
                return result;
            }
            catch (Exception ex)
            {
                _messageService.ShowError(ex.Message);
                return null;
            }
        }

        private void _view_CboSheetSelectedIndexChanged(object sender, FormEventArgs e)
        {
            DataTable xlsSheet = xlsSheetsCollection[e.msg];
            _view.ExcelTable = xlsSheet;
        }

        //private void _view_FileSaveClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string content = _view.Content;
        //        _manager.SaveContent(content, _currentFilePath);
        //        _messageService.ShowMessage("Файл успешно сохранен");
        //    }
        //    catch (Exception ex)
        //    {
        //        _messageService.ShowError(ex.Message);
        //    }
        //}

        private void _view_FileOpenClick(object sender, FormEventArgs e)
        {
            _view.ExcelTable = OpenXlFile(e.msg);
        }
        private void _view_LoadTemplatesClick(object sender, EventArgs e)
        {
            try
            {
                _model.LoadTemplate("Дебиторская_задолженность.xlsx", "Debitors_all.xlsx", "дебиторская задолженность");
            }
            catch (Exception ex)
            {
                _messageService.ShowError(ex.Message);
            }
        }
    }
}