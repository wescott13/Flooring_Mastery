using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.BLL.Helpers;
using Flooring.Models;
using Flooring.Models.Responses;

namespace Flooring_Mastery.Workflows
{
    class RemoveOrderWorkflow
    {
        public void Execute(OrderManager manager)
        {
            string orderToRemove;
            DateTime date = DateTime.MinValue;

            DisplayOrdersWorkflow displayWorkflow = new DisplayOrdersWorkflow();
            displayWorkflow.Execute(manager);
            
            DateTime dateObtained = displayWorkflow.Date();
            List<Order> orders = displayWorkflow.Orders();
            int orderInputResult = 0;

            if (dateObtained != DateTime.MinValue)
            {
                Console.WriteLine(ConsoleIO.SeparatorBar);
                Console.WriteLine("Enter the order number to remove.");
                string orderInput = Console.ReadLine();
                try
                {
                    orderInputResult = Int32.Parse(orderInput);
                }
                catch
                {
                    Console.WriteLine("{0} is not a number.", orderInput);
                }

                Order result = orders.Find(x => x.OrderNumber == orderInputResult);
                if (result != null)
                {
                    string month = result.Date.ToString("MM");  
                    string day = result.Date.ToString("dd");  

                    orderToRemove = month + day + result.Date.Year + result.OrderNumber;  

                    Order order = new Order();
                    order.OrderNumber = result.OrderNumber;
                    order.Date = result.Date;
                    order.OrderFile = orderToRemove;

                    if (ConsoleIO.GetYesNoAnswerFromUser("Remove the order?") == "Y")
                    {
                        OrderResponse removeResponse = manager.RemoveOrder(order);
                    }
                    else
                    {
                        Console.WriteLine("Add Cancelled");
                    }

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();

                }
                else
                {
                    Console.WriteLine("{0} is not an order number.", orderInputResult);
                }
            }
        }   
    }
}
