﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using _412.Messages;
using _412_check.BL;

namespace _412_check
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm form = new MainForm();
            FileManager manager = new FileManager();
            MessageService service = new MessageService();

            MainPresenter presenter = new MainPresenter(form, manager, service);
            
            Application.Run(form);


        }

        
    }
}
