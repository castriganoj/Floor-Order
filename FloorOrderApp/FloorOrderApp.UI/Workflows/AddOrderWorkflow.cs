using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.BLL;
using FloorOrderApp.Models;

namespace FloorOrderApp.UI.Workflows
{
    public class AddOrderWorkflow
    {
        private static Validation _validation = new Validation();

        public void Execute()
        {
            OrderManager manager = new OrderManager();
            Order newOrder = new Order();

            newOrder.Name = GetCustomerName();

            Tax newTax = GetState();
            Product newProduct = GetProductType();

            newOrder.Area = GetArea();
            newOrder.StateAbbr = newTax.StateAbbr;
            newOrder.TaxRate = newTax.TaxRate;
            newOrder.ProductType = newProduct.ProductType;
            newOrder.CostPerSquareFoot = newProduct.CostPerSqFt;
            newOrder.LaborCostPerSquareFoot = newProduct.LaborCostPerSqFt;

            Console.Clear();
            Console.WriteLine("New order information:\n");
            Console.WriteLine("{0, -9} - Name\n{1, -9} - State\n{2, -9} - Material\n{3, -9} - Area\n",
                newOrder.Name, newOrder.StateAbbr, newOrder.ProductType, newOrder.Area);

            if (!GetConfirmation())
                return;

            var response = manager.AddOrder(newOrder);

            if (response.Success)
            {
                Console.Clear();
                Console.WriteLine(response.Message);
                PrintNewOrder(response.Data);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Something happened when trying to add the order.");
                Console.WriteLine(response.Message);
            }

            Console.Write("Press any key to return to the menu...");
            Console.ReadKey();
        }

        private string GetCustomerName()
        {
            Console.Clear();
            string promptUser = "Enter customer name\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            return _validation.NotNull(input, promptUser);
        }

        private Tax GetState()
        {
            Console.Clear();
            string promptUser = "Available states:\n\n" +
                                "Ohio (OH), Pennsylvania (PA), Michigan (MI), Indiana (IN)\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            input = _validation.NotNull(input, promptUser);
            return _validation.IsStateValid(input, promptUser);
        }

        private Product GetProductType()
        {
            Console.Clear();
            string promptUser = "Available materials:\n\n" +
                                "Carpet, Laminate, Tile, Wood\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            input = _validation.NotNull(input, promptUser);
            return _validation.IsProductTypeValid(input, promptUser);
        }

        private decimal GetArea()
        {
            Console.Clear();
            string promptUser = "Enter the area\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            input = _validation.NotNull(input, promptUser);
            return _validation.IsDecimalValid(input, promptUser);
        }

        private bool GetConfirmation()
        {
            string promptUser = "Are you sure you want to add the new order? (Y/N)\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            if (input != null && input.ToUpper() == "Y")
                return true;

            return false;
        }

        private void PrintNewOrder(Order i)
        {
            Console.Clear();
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
            Console.WriteLine("\nTotal: {0:C}", i.TotalCost + "\n");
        }
    }
}
