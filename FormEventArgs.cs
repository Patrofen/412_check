using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _412_check
{
    public class FormEventArgs : EventArgs
    {
        public readonly string msg;
        public FormEventArgs(string message)
        {
            msg = message;
        }
    }
}
