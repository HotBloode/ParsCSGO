using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsCSGO
{
    public class Repoz
    {
        public string Code {  get;  set; }
        public DateTime Date { get; set; }

        public Repoz(string Code, DateTime Data)
        {
            this.Code = Code;
            this.Date = Data;
        }
    }
}
