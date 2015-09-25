using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.BLL;
using FloorOrderApp.Data;
using NUnit.Core;
using NUnit.Framework;
using FloorOrderApp.Models;

namespace FloorOrderApp.Tests
{
    [TestFixture]
    class OrderManagerTests
    {
        [Test]
        public void GetOrdersTestSuccess()
        {
            var manager  = new OrderManager();

            var response = manager.GetOrders("06012013");
            bool Success = response.Success;

            Assert.AreEqual(true, Success);
        }

       [Test]
        public void GetOrderTest()
        {
            var manager = new OrderManager();

            var order = manager.GetOrder("06012013", 1);

            Assert.AreEqual("Wise", order.Name);
        }

        [Test]
        public void AddOrderTest()
        {
            var manager = new OrderManager();
            var newOrder = new Order()
            {
                Area = 10,
                CostPerSquareFoot = 5.15M,
                LaborCostPerSquareFoot = 4.75M,
                Name = "Nick",
                OrderNumber = 0,
                ProductType = "Wood",
                StateAbbr = "PA",
                TaxRate = 6.75M,
            };

            var order = manager.AddOrder(newOrder);

            Assert.AreEqual("Nick", newOrder.Name);
        }

        [Test]
        public void AddOrderComputationTest()
        {
            var manager = new OrderManager();
            var newOrder = new Order()
            {
                Area = 10,
                CostPerSquareFoot = 5.15M,
                LaborCostPerSquareFoot = 4.75M,
                Name = "nick",
                OrderNumber = 0,
                ProductType = "Wood",
                StateAbbr = "PA",
                TaxRate = 6.75M,
            };

            var order = manager.AddOrder(newOrder);

            Assert.AreEqual(47.5M, newOrder.LaborCost);
            Assert.AreEqual(51.5M, newOrder.MaterialCost);
            Assert.AreEqual(105.6825, newOrder.TotalCost);
            Assert.AreEqual(6.6825, newOrder.TaxCost);
        }

        [Test]
        public void EditOrderComputationTest()
        {
            string d = "06012013";
            string expected = "Po";

            var manager = new OrderManager();
            MockOrdersRepo repo = new MockOrdersRepo();
            Order orderToEdit= repo.GetAllOrders(d)[1];

            orderToEdit.Name = expected;

            var editedOrder = manager.EditOrder(orderToEdit, d);

            Assert.AreEqual(expected, editedOrder.Data.Name);
            
        }

        [Test]
        public void RemoveOrderTest()
        {
            string d = "06012013";

            var manager = new OrderManager();
            MockOrdersRepo repo = new MockOrdersRepo();
            List<Order> OldOrders = repo.GetAllOrders(d);
            Order orderToRemove = repo.GetAllOrders(d)[0];

            manager.RemoveOrder(d, orderToRemove);
            List<Order> expected = repo.GetAllOrders(d);

            Assert.False(OldOrders.Contains(orderToRemove));

        }
    }
}
