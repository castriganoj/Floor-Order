using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorOrderApp.Models
{
    public class Product
    {
        public string ProductType { get; set; }
        public decimal CostPerSqFt { get; set; }
        public decimal LaborCostPerSqFt { get; set; }
    }
}
