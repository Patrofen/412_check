﻿using System;
using System.Data;
using _412_check.BL;
using _412.Messages;

namespace _412_check
{
    class MainPresenter
    {
        private readonly IMainForm _view;
        private readonly IFileManager _manager;
        private readonly IMessageService _messageService;
        public string _currentFilePath;
        private DataTableCollection XlsSheetsCollection;

        public MainPresenter(IMainForm view, IFileManager manager, IMessageService service)
        {
            _view = view;
            _manager = manager;
            _messageService = service;
            _view.FileOpenClick += _view_FileOpenClick;
            _view.CboSheetSelectedIndexChanged += _view_CboSheetSelectedIndexChanged; ;
            //_view.FileSaveClick += _view_FileSaveClick;
        }

        private void _view_CboSheetSelectedIndexChanged(object sender, FormEventArgs e)
        {
            DataTable xlsSheet = XlsSheetsCollection[e.msg];
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

        private void _view_FileOpenClick(object sender, EventArgs e)
        {
            try
            {
                string filePath = _view.FilePath;
                bool isExist = _manager.IsExist(filePath);
                if (!isExist)
                {
                    _messageService.ShowExclmation("Выбранный файл не существует");
                    return;
                }
                _currentFilePath = filePath;
                XlsSheetsCollection = _manager.GetContent(filePath);
                _view.SetSheetList(XlsSheetsCollection);
            }
            catch (Exception ex)
            {
                _messageService.ShowError(ex.Message);
            }
        }
    }
}