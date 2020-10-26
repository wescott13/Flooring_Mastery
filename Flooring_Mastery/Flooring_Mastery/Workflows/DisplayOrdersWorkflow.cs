using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.BLL.Helpers;
using Flooring.Models;
using Flooring.Models.Responses;

namespace Flooring_Mastery.Workflows
{
    public class DisplayOrdersWorkflow
    {
        DateTime dateObtained { get; set; }
        DateTime date;
        List<Order> orders { get; set; }
        public void Execute(OrderManager manager)
        {
            Console.Clear();
            Console.WriteLine(ConsoleIO.SeparatorBar);

            Console.Write("\nEnter date to display orders: ");
            string dateInput = Console.ReadLine();

            if (DateTime.TryParse(dateInput, out date))
            {
                OrderDateLookupResponse response = manager.LookupOrderDate(date);
                if (response.Success)
                {
                    if (response.Orders.Count > 0)
                    {
                        orders = response.Orders;

                        foreach (Order order in orders)
                        {
                            ConsoleIO.DisplayOrderDetails(order);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No orders found on {0}.", date);
                    date = DateTime.MinValue;
                }
            }
            else
            {
                Console.WriteLine("{0} is not a valid date.", dateInput);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        public DateTime Date()
        {
            dateObtained = date;
            return dateObtained;
        }
        public List<Order> Orders()
        {
            return orders;
        }

    }
}
