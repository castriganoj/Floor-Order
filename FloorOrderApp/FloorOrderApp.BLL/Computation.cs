using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.Models;

namespace FloorOrderApp.BLL
{
    public class Computation
    {
        // The OrderManager will call GetTotals, passing in the new order, and will get back the order with the updated params
        public Order GetTotals(Order order)
        {
            order.LaborCost = order.LaborCostPerSquareFoot * order.Area;
            order.MaterialCost = order.CostPerSquareFoot * order.Area;
            order.TaxCost = (order.LaborCost + order.MaterialCost) * (order.TaxRate / 100);
            order.TotalCost = order.LaborCost + order.MaterialCost + order.TaxCost;
            return order;
        }
    }
}
