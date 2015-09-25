using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace FloorOrderApp.Models
{
    public class Tax
    {
        public string StateAbbr { get; set; }
        public string State { get; set; }
        public decimal TaxRate { get; set; }
    }
}
