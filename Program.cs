using System;
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
            BusinessLogic bsnsLogic = new BusinessLogic();
            MessageService service = new MessageService();

            MainPresenter presenter = new MainPresenter(form, bsnsLogic, service);
            
            Application.Run(form);
        }

        
    }
}
