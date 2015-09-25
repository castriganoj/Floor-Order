using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.BLL;
using FloorOrderApp.Models;

namespace FloorOrderApp.UI.Workflows
{
    public class EditOrderWorkflow
    {
        private static Validation _validation = new Validation();

        public void Execute()
        {
            OrderManager manager = new OrderManager();

            string date;
            Order selectedOrder;

            do
            {
                date = GetDate();
                var orderNumber = GetOrderNumber();
                selectedOrder = manager.GetOrder(date, orderNumber);

                if (selectedOrder == null)
                {
                    Console.Clear();
                    Console.WriteLine("Order does not exist.");
                    Console.Write("\nPress any key to continue, or (Q) to return to main menu...");

                    string input = Console.ReadLine();

                    if (input != null && input.ToUpper() == "Q")
                        return;
                }
            } while (selectedOrder == null);

            PrintOrder(selectedOrder);

            Console.Write("\nPress enter to begin editing...");
            Console.ReadLine();

            selectedOrder = GetCustomerName(selectedOrder);
            selectedOrder = GetState(selectedOrder);
            selectedOrder = GetProductType(selectedOrder);
            selectedOrder = GetArea(selectedOrder);

            var response = manager.EditOrder(selectedOrder, date);

            if (response.Success)
            {
                Console.Clear();
                Console.WriteLine(response.Message);
                Console.WriteLine();
                PrintEditedOrder(response.Data);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Something happened.");
                Console.WriteLine(response.Message);
            }

            Console.Write("\nPress any key to continue...");
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

        private void PrintOrder(Order selectedOrder)
        {
            Console.Clear();

            Console.WriteLine("Order Number:  - - - - - - - " + selectedOrder.OrderNumber);
            Console.WriteLine("Customer Name: > > > > > > > " + selectedOrder.Name);
            Console.WriteLine("State: - - - - - - - - - - - " + selectedOrder.StateAbbr);
            Console.WriteLine("Tax Rate:  > > > > > > > > > {0:P}", selectedOrder.TaxRate / 100);
            Console.WriteLine("Product Type:  - - - - - - - " + selectedOrder.ProductType);
            Console.WriteLine("Area:  > > > > > > > > > > > " + selectedOrder.Area);
            Console.WriteLine("Cost Per Square Foot:  - - - {0:C}", selectedOrder.CostPerSquareFoot);
            Console.WriteLine("Labor Cost Per Square Foot:  {0:C}", selectedOrder.LaborCostPerSquareFoot);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("\nTotal material cost is:  > > {0:C}", selectedOrder.MaterialCost);
            Console.WriteLine("Total labor cost is: - - - - {0:C}", selectedOrder.LaborCost);
            Console.WriteLine("Total tax cost is: > > > > > {0:C}\n", selectedOrder.TaxCost);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("\nTotal: {0:C}", selectedOrder.TotalCost);
            Console.WriteLine("\n==========================================\n");
        }

        // Here down starts the editing

        private Order GetCustomerName(Order selectedOrder)
        {
            Console.Clear();
            string promptUser = "To keep a field unchanged, leave it blank and press enter.\n\n" +
                                "Enter new customer name (current name: " + selectedOrder.Name + ")\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            if (input == "")
                return selectedOrder;

            selectedOrder.Name = input;
            return selectedOrder;
        }

        private Order GetState(Order selectedOrder)
        {
            Console.Clear();
            string promptUser = "To keep a field unchanged, leave it blank and press enter.\n\n" +
                                "Available states (current state: " + selectedOrder.StateAbbr + "):\n\n" +
                                "Ohio (OH), Pennsylvania (PA), Michigan (MI), Indiana (IN)\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            if (input == "")
                return selectedOrder;

            input = _validation.NotNull(input, promptUser);

            Tax tax = _validation.IsStateValid(input, promptUser);

            selectedOrder.StateAbbr = tax.StateAbbr;
            return selectedOrder;
        }

        private Order GetProductType(Order selectedOrder)
        {
            Console.Clear();
            string promptUser = "To keep a field unchanged, leave it blank and press enter.\n\n" +
                                "Available materials (current material: " + selectedOrder.ProductType + "):\n\n" +
                                "Carpet, Laminate, Tile, Wood\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            if (input == "")
                return selectedOrder;

            input = _validation.NotNull(input, promptUser);

            Product product = _validation.IsProductTypeValid(input, promptUser);

            selectedOrder.ProductType = product.ProductType;
            return selectedOrder;
        }

        private Order GetArea(Order selectedOrder)
        {
            Console.Clear();
            string promptUser = "To keep a field unchanged, leave it blank and press enter.\n\n" +
                                "Enter the new area (current area: " + selectedOrder.Area + ")\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            if (input == "")
                return selectedOrder;

            selectedOrder.Area = _validation.IsDecimalValid(input, promptUser);
            return selectedOrder;
        }

        private void PrintEditedOrder(Order i)
        {
            Console.WriteLine("Order Number:  - - - - - - - " + i.OrderNumber);
            Console.WriteLine("Customer Name: > > > > > > > " + i.Name);
            Console.WriteLine("State: - - - - - - - - - - - " + i.StateAbbr);
            Console.WriteLine("Tax Rate:  > > > > > > > > > {0:P}", i.TaxRate / 100);
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
        }
    }
}
