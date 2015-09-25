using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.BLL;
using FloorOrderApp.Models;

namespace FloorOrderApp.UI.Workflows
{
    public class DisplayOrdersWorkflow
    {
        private static Validation _validation = new Validation();

        public void Execute()
        {
            OrderManager manager = new OrderManager();
           
            Console.Clear();

            string date = GetDate();
            var response = manager.GetOrders(date);

            Console.Clear();

            if (response.Success)
            {
                PrintOrders(response.Data, date);
            }

            else
            {
                Console.Clear();
                Console.WriteLine(response.Message);
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private string GetDate()
        {
            string promptUser = "Enter the date the order was placed on (MMDDYYYY)\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            input = _validation.NotNull(input, promptUser);
            return _validation.IsDateValid(input, promptUser);
        }

        private void PrintOrders(OrderList orderList, string d)
        {
            var o = orderList.Orders;
            
            DateTime fDate = DateTime.Parse(d.Substring(0, 2) + "/" + d.Substring(2, 2) + "/" + d.Substring(4, 4));

            Console.WriteLine("Orders placed on " + fDate.ToString("D") + "\n");

            foreach (var i in o)
            {
                Console.WriteLine("Order Number:  - - - - - - - " + i.OrderNumber);
                Console.WriteLine("Customer Name: > > > > > > > " + i.Name);
                Console.WriteLine("State: - - - - - - - - - - - " + i.StateAbbr);
                Console.WriteLine("Tax Rate:  > > > > > > > > > {0:P}", i.TaxRate/100);
                Console.WriteLine("Product Type:  - - - - - - - " + i.ProductType);
                Console.WriteLine("Area:  > > > > > > > > > > > " + i.Area);
                Console.WriteLine("Cost Per Square Foot:  - - - {0:C}", i.CostPerSquareFoot);
                Console.WriteLine("Labor Cost Per Square Foot:  {0:C}", i.LaborCostPerSquareFoot);
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("\nTotal material cost is:  > > {0:C}", i.MaterialCost);
                Console.WriteLine("Total labor cost is: - - - - {0:C}", i.LaborCost);
                Console.WriteLine("Total tax cost is: > > > > > {0:C}\n", i.TaxCost);
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("\nTotal: {0:C}", i.TotalCost);   
                Console.WriteLine("\n==========================================\n");
            }
        }
    }
}
