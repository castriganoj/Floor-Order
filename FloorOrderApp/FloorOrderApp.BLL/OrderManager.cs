using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.Data;
using FloorOrderApp.Data.Interfaces;
using FloorOrderApp.Models;

namespace FloorOrderApp.BLL
{
    public class OrderManager
    {
        private IOrderRepo _repo = RepoFactory.GetRepo();

        public Response<OrderList> GetOrders(string date)
        {
            Response<OrderList> response = new Response<OrderList>();

            try
            {
                if (!OrderExist(date))
                {
                    response.Success = false;
                    response.Message = "This order does not exist";
                }
                else
                {
                    OrderList orderListObj = new OrderList();
                    orderListObj.Orders = _repo.GetAllOrders(date);
                    response.Success = true;
                    response.Data = orderListObj;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error occurred.";
                _repo.LogError(ex);
            }

            return response;
        }

        public Order GetOrder(string date, int orderNumber)
        {
            if (!OrderExist(date, orderNumber))
            {
                return null;
            }

            List<Order> ordersList = _repo.GetAllOrders(date);

            return ordersList.Where(o => o.OrderNumber == orderNumber).ToList().FirstOrDefault();
        }

        public Response<Order> AddOrder(Order newOrder)
        {
            Response<Order> response = new Response<Order>();

            try
            {
                Computation totals = new Computation();

                newOrder = totals.GetTotals(newOrder);
                response.Data = _repo.CreateOrder(newOrder);
                response.Success = true;
                response.Message = "Order successfully added.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error occurred.";
                _repo.LogError(ex);
            }

            return response;
        }

        public Response<Order> EditOrder(Order selectedOrder, string date)
        {
            Response<Order> response = new Response<Order>();

            try
            {
                Computation totals = new Computation();

                selectedOrder = totals.GetTotals(selectedOrder);
                response.Data = _repo.EditOrder(selectedOrder, date);
                response.Success = true;
                response.Message = "Order successfully updated.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error occurred.";
                _repo.LogError(ex);
            }

            return response;
        }

        public Response<Order> RemoveOrder(string d, Order orderToRemove)
        {
            Response<Order> response = new Response<Order>();

            try
            {
                List<Order> ordersList = _repo.GetAllOrders(d);
                ProdOrdersRepo pRepo = new ProdOrdersRepo();

                int indexToRemove = 0;

                foreach (var t in ordersList.Where(t => t.OrderNumber == orderToRemove.OrderNumber))
                {
                    indexToRemove = ordersList.IndexOf(t);
                }

                pRepo.LogDeletedOrder(ordersList[indexToRemove]);

                ordersList.RemoveAt(indexToRemove);

                _repo.UpdateFile(ordersList, d);

                response.Success = true;
                response.Message = "Order successfully removed.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error occurred.";
                _repo.LogError(ex);
            }

            return response;
        }

        public void GetOrderFile(string date)
        {
            ProdOrdersRepo repo = new ProdOrdersRepo();
            repo.OpenOrderFile(date);
        }

        public List<Tax> GetStates()
        {
            return _repo.GetSampleTaxes();
        }

        public List<Product> GetMaterials()
        {
            return _repo.GetSampleProducts();
        }

        public bool OrderExist(string d)
        {
            List<Order> ordersList = _repo.GetAllOrders(d);

            if (ordersList == null)
                return false;

            return true;
        }

        public bool OrderExist(string d, int o)
        {
            List<Order> ordersList = _repo.GetAllOrders(d);

            if (ordersList == null)
                return false;

            return ordersList.Any(order => order.OrderNumber == o);
        }

        public string GetCurrentMode()
        {
            if (_repo.GetType() == typeof(MockOrdersRepo))
                return "Mock";

            return "Production";
        }

        public void PurgeMockData()
        {
            MockOrdersRepo repo = new MockOrdersRepo();
            repo.PurgeMockDataFolder();
        }
    }
}
