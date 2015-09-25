using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.BLL;
using FloorOrderApp.Models;

namespace FloorOrderApp.UI.Workflows
{
    public class RemoveOrderWorkflow
    {
        private static Validation _validation = new Validation();

        public void Execute()
        {
            OrderManager manager = new OrderManager();

            Order orderToRemove;
            string date;

            do
            {
                date = GetDate();
                var orderNumber = GetOrderNumber();
                orderToRemove = manager.GetOrder(date, orderNumber);

                if (orderToRemove == null)
                {
                    Console.Clear();
                    Console.WriteLine("Order does not exist.");
                    Console.Write("\nPress any key to continue, or (Q) to return to main menu...");

                    string input = Console.ReadLine();

                    if (input != null && input.ToUpper() == "Q")
                        return;
                }
            } while (orderToRemove == null);

            PrintOrderToRemove(orderToRemove);

            if (!GetConfirmation())
                return;

            var response = manager.RemoveOrder(date, orderToRemove);

            if (response.Success)
            {
                Console.Clear();
                Console.WriteLine(response.Message);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("A problem occurred...");
                Console.WriteLine(response.Message);
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private string GetDate()
        {
            Console.Clear();
            string promptUser = "Enter the date the order was placed on (MMDDYYYY)\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            input = _validation.NotNull(input, promptUser);
            return _validation.IsDateValid(input, promptUser);
        }

        private int GetOrderNumber()
        {
            Console.Clear();
            string promptUser = "Enter the order number\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            input = _validation.NotNull(input, promptUser);
            return _validation.IsIntegerValid(input, promptUser);
        }

        private void PrintOrderToRemove(Order orderToRemove)
        {
            Console.Clear();

            Console.WriteLine("Order Number:  - - - - - - - " + orderToRemove.OrderNumber);
            Console.WriteLine("Customer Name: > > > > > > > " + orderToRemove.Name);
            Console.WriteLine("State: - - - - - - - - - - - " + orderToRemove.StateAbbr);
            Console.WriteLine("Tax Rate:  > > > > > > > > > {0:P}", orderToRemove.TaxRate / 100);
            Console.WriteLine("Product Type:  - - - - - - - " + orderToRemove.ProductType);
            Console.WriteLine("Area:  > > > > > > > > > > > " + orderToRemove.Area);
            Console.WriteLine("Cost Per Square Foot:  - - - {0:C}", orderToRemove.CostPerSquareFoot);
            Console.WriteLine("Labor Cost Per Square Foot:  {0:C}", orderToRemove.LaborCostPerSquareFoot);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("\nTotal material cost is:  > > {0:C}", orderToRemove.MaterialCost);
            Console.WriteLine("Total labor cost is: - - - - {0:C}", orderToRemove.LaborCost);
            Console.WriteLine("Total tax cost is: > > > > > {0:C}\n", orderToRemove.TaxCost);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("\nTotal: {0:C}", orderToRemove.TotalCost);
            Console.WriteLine("\n==========================================\n");
        }

        private bool GetConfirmation()
        {
            string promptUser = "Are you sure you want to remove this order? (Y/N)\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            if (input != null && input.ToUpper() == "Y")
                return true;

            return false;
        }
    }
}
