using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.BLL.Helpers;
using Flooring.Models.Responses;

namespace Flooring.UI.Workflows
{
    public class OrdersDateLookupWorkflow
    {
        public void Execute()
        {
            OrderManager manager = OrderManagerFactory.Create();

            Console.Clear();
            Console.WriteLine(ConsoleIO.SeparatorBar);
            
            Console.Write("\nEnter date to display orders: ");
            string dateInput = Console.ReadLine();

            DateTime date;

            try 
            {
                DateTime.TryParse(dateInput, out date);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not a valid date.", dateInput);
                
            }
            
        }
    }
}
