using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _412_check.BL
{
    public class Currency
    {
        public string Code { get; set; }
        public string LiteralCode { get; set; }
        public string NumericCode { get; set; }

        public Currency() { }
        public Currency(string code, string literalCode, string numericCode)
        {
            Code = code;
            LiteralCode = literalCode;
            NumericCode = numericCode;
        }
    }
}
