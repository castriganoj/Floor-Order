using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.Data.Interfaces;
using FloorOrderApp.Models;

namespace FloorOrderApp.Data
{
    public class ProdOrdersRepo : IOrderRepo
    {
        private static string _today = DateTime.Today.ToString("MMddyyyy");

        public Order CreateOrder(Order o)
        {
            string f = @"ProdData\Orders_" + _today + ".txt";

            var ordersList = GenerateOrderNumber(o);

            WriteToFile(f, ordersList);

            return o;
        }

        public Order EditOrder(Order o, string d)
        {
            string f = @"ProdData\Orders_" + d + ".txt";

            var ordersList = GetAllOrders(d);
            int indexToUpdate = 0;

            foreach (var t in ordersList.Where(t => t.OrderNumber == o.OrderNumber))
            {
                indexToUpdate = ordersList.IndexOf(t);
            }

            ordersList[indexToUpdate] = o;

            WriteToFile(f, ordersList);

            return o;
        }

        public List<Order> GetAllOrders(string d)
        {

            string filePath = @"ProdData\Orders_" + d + ".txt";

            List<Order> orders = new List<Order>();

            if (!File.Exists(filePath))
                return null;

            var reader = File.ReadAllLines(filePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var order = new Order
                {
                    OrderNumber = int.Parse(columns[0]),
                    Name = columns[1],
                    StateAbbr = columns[2],
                    TaxRate = decimal.Parse(columns[3]),
                    ProductType = (columns[4]),
                    Area = decimal.Parse(columns[5]),
                    CostPerSquareFoot = decimal.Parse(columns[6]),
                    LaborCostPerSquareFoot = decimal.Parse(columns[7]),
                    MaterialCost = decimal.Parse(columns[8]),
                    LaborCost = decimal.Parse(columns[9]),
                    TaxCost = decimal.Parse(columns[10]),
                    TotalCost = decimal.Parse(columns[11])
                };

                orders.Add(order);
            }

            return orders;
        }

        public void UpdateFile(List<Order> ordersList, string d)
        {
            string f = @"ProdData\Orders_" + d + ".txt";

            WriteToFile(f, ordersList);
        }

        public List<Product> GetSampleProducts()
        {
            List<Product> sampleProducts= new List<Product>();

            var reader = File.ReadAllLines(@"ProdSampleData\Products.txt");

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var prods = new Product()
                {
                    ProductType = columns[0],
                    CostPerSqFt = decimal.Parse(columns[1]),
                    LaborCostPerSqFt = decimal.Parse(columns[2])
                };

                sampleProducts.Add(prods);
            }


            return sampleProducts;
        }

        public List<Tax> GetSampleTaxes()
        {
            List<Tax> sampleTaxes = new List<Tax>();

            var reader = File.ReadAllLines(@"ProdSampleData\Taxes.txt");

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var tax = new Tax()
                {
                    StateAbbr = columns[0],
                    State = columns[1],
                    TaxRate = decimal.Parse(columns[2])
                };

                sampleTaxes.Add(tax);
            }


            return sampleTaxes;
        }

        // Adds order to list as well, and returns it
        public List<Order> GenerateOrderNumber(Order newOrder)
        {
            FileInfo f = new FileInfo(@"ProdData\Orders_" + _today + ".txt");

            var ordersList = new List<Order>();

            if (f.Exists)
            {
                ordersList = GetAllOrders(_today);
                newOrder.OrderNumber = ordersList.Last().OrderNumber + 1;
            }
            else
            {
                newOrder.OrderNumber = 1;
            }

            ordersList.Add(newOrder);

            return ordersList;
        }

        private void WriteToFile(string f, List<Order> ordersList)
        {
            using (var writer = File.CreateText(f))
            {
                writer.WriteLine(
                    "OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");

                foreach (var order in ordersList)
                {
                    writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", order.OrderNumber,
                        order.Name, order.StateAbbr, order.TaxRate, order.ProductType, order.Area,
                        order.CostPerSquareFoot, order.LaborCostPerSquareFoot, order.MaterialCost, order.LaborCost,
                        order.TaxCost, order.TotalCost);
                }
            }
        }

        public void OpenOrderFile(string date)
        {
            string file = @"ProdData\Orders_" + date + ".txt";
            Process.Start(file);
        }

        public void LogError(Exception ex)
        {
            List<string> errorsLog = new List<string>();
            string[] reader;

            if (File.Exists("log.txt"))
            {
                reader = File.ReadAllLines("log.txt");

                errorsLog.AddRange(reader);
            }

            errorsLog.Add(ex.Message);

            using (var writer = File.CreateText("log.txt"))
            {
                foreach (var error in errorsLog)
                    writer.WriteLine(DateTime.Now + ": " + error + "\n");
            }
        }

        public void LogDeletedOrder(Order order)
        {
            List<string> deletedOrdersLog = new List<string>();
            string[] reader;

            if (File.Exists("deleted_orders_log.txt"))
            {
                reader = File.ReadAllLines("deleted_orders_log.txt");

                deletedOrdersLog.AddRange(reader);
            }

            string orderConcat =
                $"{order.OrderNumber},{order.Name},{order.StateAbbr},{order.TaxRate},{order.ProductType},{order.Area},{order.CostPerSquareFoot}," +
                $"{order.LaborCostPerSquareFoot},{order.MaterialCost},{order.LaborCost},{order.TaxCost},{order.TotalCost}";

            deletedOrdersLog.Add(orderConcat);

            using (var writer = File.CreateText("deleted_orders_log.txt"))
            {
                foreach (var o in deletedOrdersLog)
                    writer.WriteLine(o);
            }
        }
    }
}
