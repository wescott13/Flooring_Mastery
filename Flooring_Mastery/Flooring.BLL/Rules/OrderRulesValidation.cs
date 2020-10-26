using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;
using Flooring.Models.Interfaces;

namespace Flooring.BLL.Rules
{
    public class OrderRulesValidation : IOrderRepository
    {
        public List<Order> DisplayOrdersByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Order LoadOrder(DateTime date, int orderNumber)
        {
            throw new NotImplementedException();
        }

        public void RemoveOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void SaveOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
