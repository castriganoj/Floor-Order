using FloorOrderApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.Data.Interfaces;
using Microsoft.Practices.Unity;

namespace FloorOrderApp.Data
{

    public class MockOrdersRepo : IOrderRepo
    {
        private static string _today = DateTime.Today.ToString("MMddyyyy");

        //public UnityContainer GetAllOrders(string d)
        //{
            
        //}

        public Order CreateOrder(Order o)
        {
            string f = @"MockData\Orders_" + _today + ".txt";

            var ordersList = GenerateOrderNumber(o);

            WriteToFile(f, ordersList);

            return o;
        }

        public Order EditOrder(Order o, string d)
        {
            string f = @"MockData\Orders_" + d + ".txt";

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

            string filePath = @"MockData\Orders_" + d + ".txt";

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
            string f = @"MockData\Orders_" + d + ".txt";

            WriteToFile(f, ordersList);
        }

        public void PurgeMockDataFolder()
        {
            DirectoryInfo d = new DirectoryInfo("MockData");

            var files = d.EnumerateFiles();

            foreach (var file in files)
            {
                File.Delete(@"MockData\" + file.Name);
            }
        }

        public List<Product> GetSampleProducts()
        {
            List<Product> sampleProducts = new List<Product>
            {
                new Product {ProductType = "Carpet", CostPerSqFt = 2.25M, LaborCostPerSqFt = 2.10M},
                new Product {ProductType = "Laminate", CostPerSqFt = 1.75M, LaborCostPerSqFt = 2.10M},
                new Product {ProductType = "Tile", CostPerSqFt = 3.50M, LaborCostPerSqFt = 4.15M},
                new Product {ProductType = "Wood", CostPerSqFt = 5.15M, LaborCostPerSqFt = 4.75M}
            };

            return sampleProducts;
        }

        public List<Tax> GetSampleTaxes()
        {
            List<Tax> sampleTaxes = new List<Tax>
            {
                new Tax {StateAbbr = "OH", State = "Ohio", TaxRate = 6.25M},
                new Tax {StateAbbr = "PA", State = "Pennsylvania", TaxRate = 6.75M},
                new Tax {StateAbbr = "MI", State = "Michigan", TaxRate = 5.75M},
                new Tax {StateAbbr = "IN", State = "Indiana", TaxRate = 6.00M}
            };

            return sampleTaxes;
        }

        // Adds order to list as well, and returns it
        public List<Order> GenerateOrderNumber(Order newOrder)
        {
            FileInfo f = new FileInfo(@"MockData\Orders_" + _today + ".txt");

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
                    writer.WriteLine($"{order.OrderNumber},{order.Name},{order.StateAbbr},{order.TaxRate},{order.ProductType},{order.Area},{order.CostPerSquareFoot}," +
                                     $"{order.LaborCostPerSquareFoot},{order.MaterialCost},{order.LaborCost},{order.TaxCost},{order.TotalCost}");
                }
            }
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
    }
}
