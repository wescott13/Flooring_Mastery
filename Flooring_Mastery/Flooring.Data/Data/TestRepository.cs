using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;
using Flooring.Models.Interfaces;

namespace Flooring.Data.Data
{
    public class TestRepository : IOrderRepository
    {
        Dictionary<string, Order> orders;
        
        public TestRepository()
        {
            orders = new Dictionary<string, Order>();

            populateOrders();
        }

        public List<Order> DisplayOrdersByDate(DateTime date)
        {
            string fileDate = date.ToString("MMddyyyy");
            return orders.Where(orderDate => orderDate.Key.Contains(fileDate)).Select(orderDate => orderDate.Value).ToList();
        }

        public void EditOrder(Order order)
        {
            string date = order.Date.ToString("MMddyyyy");
            orders[date + order.OrderNumber] = order;
        }
        public void RemoveOrder(Order order)
        {
            string date = order.Date.ToString("MMddyyyy");
            orders.Remove(date + order.OrderNumber);
        }
        public void SaveOrder(Order order)
        {
            string date = order.Date.ToString("MMddyyyy");
            orders.Add(date + order.OrderNumber, order);
        }
        private void populateOrders()
        {
            orders.Add("010120251", new Order()
            {
                Date = DateTime.Parse("01/01/2025"),
                OrderNumber = 1,
                CustomerName = "TestCustomer",
                State = "OH",
                TaxRate = 6.25M,
                ProductType = "Wood",
                Area = 100.00M,
                CostPerSquareFoot = 5.15M,
                LaborCostPerSquareFoot = 4.75M,
                MaterialCost = 515.00M,
                LaborCost = 475.00M,
                Tax = 61.88M,
                Total = 1051.88M
            });
            orders.Add("010120252", new Order()
            {
                Date = DateTime.Parse("01/01/2025"),
                OrderNumber = 2,
                CustomerName = "Second TestCustomer",
                State = "OH",
                TaxRate = 6.25M,
                ProductType = "Wood",
                Area = 100.00M,
                CostPerSquareFoot = 5.15M,
                LaborCostPerSquareFoot = 4.75M,
                MaterialCost = 515.00M,
                LaborCost = 475.00M,
                Tax = 61.88M,
                Total = 1051.88M
            });
            orders.Add("010120131", new Order()
            {
                Date = DateTime.Parse("01/01/2013"),
                OrderNumber = 1,
                CustomerName = "PastTestCustomer",
                State = "OH",
                TaxRate = 6.25M,
                ProductType = "Wood",
                Area = 100.00M,
                CostPerSquareFoot = 5.15M,
                LaborCostPerSquareFoot = 4.75M,
                MaterialCost = 515.00M,
                LaborCost = 475.00M,
                Tax = 61.88M,
                Total = 1051.88M
            });
        }
    }
}
