using FloorOrderApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace FloorOrderApp.Data.Interfaces

{
    public interface IOrderRepo
    {
        void LogError(Exception ex);

        List<Order> GetAllOrders(string d);

        List<Product> GetSampleProducts();

        List<Tax> GetSampleTaxes();

        Order CreateOrder(Order o);

        Order EditOrder(Order o, string d);

        List<Order> GenerateOrderNumber(Order newOrder);

        void UpdateFile(List<Order> ordersList, string d);

        //UnityContainer GetAllOrders(string d);
    }
}
