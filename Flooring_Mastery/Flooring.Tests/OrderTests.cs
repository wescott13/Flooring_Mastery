using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.BLL.Helpers;
using Flooring.BLL.Rules;
using Flooring.Data.Data;
using Flooring.Models;
using Flooring.Models.Interfaces;
using Flooring.Models.Responses;
using NUnit.Framework;

namespace Flooring.Tests
{
    [TestFixture]
    public class OrderTests
    {
        [TestCase(200.00,"")]
        [TestCase(90.00,"Error:  Area must be greater than 100 Square Feet.")]
        public void orderAreaTest(decimal Area, string expectedResult)
        {
            Order order = new Order();
            order.Area = Area;

            string response = AddOrderRules.OrderArea(order.Area);

            Assert.AreEqual(expectedResult, response);
        }
        [TestCase("1/21/2025","")]
        [TestCase("1/21/2000","Error:  Date must be in the future.")]

        public void orderFutureDateTest(DateTime Date, string expectedResult)
        {
            Order order = new Order();
            order.Date = Date;

            string response = AddOrderRules.OrderDate(order.Date);

            Assert.AreEqual(expectedResult, response);
        }
        [Test]
        public void AddOrder()
        {
            OrderManager manager = new OrderManager(new TestRepository(), new StateTaxesRepository(), new ProductsRepository());
            
            Order order = new Order();

            order.Date = DateTime.Parse("01/21/2025");
            order.OrderNumber = 1;
            order.CustomerName = "AddTester";
            order.State = "OH";
            order.TaxRate = 6.25M;
            order.ProductType = "Wood";
            order.Area = 200.00M;
            order.CostPerSquareFoot = 5.15m;
            order.LaborCostPerSquareFoot = 4.75m;
            order.MaterialCost = 1030m;
            order.LaborCost = 950m;
            order.Tax = 124;
            order.Total = 2104m;

            OrderResponse response = manager.AddOrder(order);

            Assert.IsNotNull(response.Order);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.Order.OrderNumber);
        }
        [Test]

        public void RemoveOrder()
        {
            OrderManager manager = new OrderManager(new TestRepository(), new StateTaxesRepository(), new ProductsRepository());
            Order order = new Order();

            OrderResponse response = manager.RemoveOrder(order);

            Assert.IsNotNull(response.Order);
            Assert.IsTrue(response.Success);  
        }
        [Test]
        public void EditOrder()
        {
            OrderManager manager = new OrderManager(new TestRepository(), new StateTaxesRepository(), new ProductsRepository());

            Order order = new Order();

            OrderResponse response = manager.EditOrder(order);

            Assert.IsNotNull(response.Order);
            Assert.IsTrue(response.Success);
        }

        [TestCase("1/1/2025", true)]
        [TestCase("1/21/2000", false)]

        public void orderDateLookupTest(DateTime Date, bool expectedResult)
        {
            OrderManager manager = new OrderManager(new TestRepository(), new StateTaxesRepository(), new ProductsRepository());

            OrderDateLookupResponse response = manager.LookupOrderDate(Date);

            Assert.IsTrue(response.Success == expectedResult);
        }
    }
}
