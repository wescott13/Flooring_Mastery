using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Interfaces
{
    public interface IOrderRepository
    {
        //Defining what actions a order repository should be able to take.
        List<Order> DisplayOrdersByDate(DateTime date);
        void SaveOrder(Order order);
        void RemoveOrder(Order order);
        void EditOrder(Order order);
    }
}
