using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.BLL;
using FloorOrderApp.Data;
using FloorOrderApp.Models;
using NUnit.Framework;

namespace FloorOrderApp.Tests
{
    [TestFixture]
    class OrderRepositoryTests
    {
        [Test]
        public void CanLoadAllOrdersForDate()
        {
            var repo = new MockOrdersRepo();

            var orders = repo.GetAllOrders("06012013");
            var wise = orders[1].Name;

            Assert.AreEqual("Ward", wise);
        }

        [Test]
        public void CanAddOrder()
        {
            var repo = new MockOrdersRepo();

            var newOrder = new Order()
            {
                Area = 10,
                CostPerSquareFoot = 5.15M,
                LaborCost = 47.50M,
                LaborCostPerSquareFoot = 4.75M,
                MaterialCost = 51.50M,
                Name = "nick",
                OrderNumber = 0,
                ProductType = "Wood",
                StateAbbr = "PA",
                TaxCost = 668.2500M,
                TaxRate = 6.75M,
                TotalCost = 767.2500M
            };

            repo.CreateOrder(newOrder);
            List<Order> orders = repo.GetAllOrders(DateTime.Today.ToString("MMddyyyy"));

            Assert.AreEqual(newOrder.TotalCost, orders.Last().TotalCost);
            Assert.AreEqual(newOrder.Name, orders.Last().Name);
        }

        [Test]
        public void CanEditOrderWithoutChanging()
        {
            var repo = new MockOrdersRepo();
            var date = "06012013";
            var orders = repo.GetAllOrders(date);
            var order = orders[2];

            var updatedOrder = repo.EditOrder(order, date);
            
            Assert.AreEqual("Woods", updatedOrder.Name);
            Assert.AreEqual(475.00M, updatedOrder.LaborCost);
        }

        [Test]
        public void CanEditOrder()
        {
            var repo = new MockOrdersRepo();
            var date = "06012013";
            var orders = repo.GetAllOrders(date);
            var order = orders[0];

            order.ProductType = "Laminate";
            order.Name = "Carol";

            var updatedOrder = repo.EditOrder(order, date);

            Assert.AreNotEqual("Wise", updatedOrder.Name);
            Assert.AreNotEqual("Wood", updatedOrder.ProductType);
        }

        [Test]
        public void CanRemoveOrder()
        {
            var repo = new MockOrdersRepo();
            List<Order> list = repo.GetAllOrders("06012013");

            List<Order> newList = new List<Order>();

            //remove 4th order from repo
            newList.Add(list[0]);
            newList.Add(list[1]);
            newList.Add(list[2]);


            repo.UpdateFile(newList, "06012013");
            List<Order> updatedList = repo.GetAllOrders("06012013");


            Assert.False(newList[0] == updatedList[0]);
           
        }


    }
}
