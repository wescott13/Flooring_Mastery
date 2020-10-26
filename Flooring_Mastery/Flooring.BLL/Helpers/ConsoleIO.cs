using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;

namespace Flooring.BLL.Helpers
{
    public static class ConsoleIO
    {
        public const string SeparatorBar = "******************************";

        public static void DisplayOrderDetails(Order order)
        {
            Console.WriteLine("{0} | {1}", order.OrderNumber, order.Date.ToString("MM"+"/"+"dd"+"/"+"yyyy"));
            Console.WriteLine(order.CustomerName);
            Console.WriteLine(order.State);
            Console.WriteLine("Product: {0}", order.ProductType);
            Console.WriteLine("Materials: {0:c}", order.MaterialCost);
            Console.WriteLine("Labor:  {0:c}", order.LaborCost);
            Console.WriteLine("Tax:  {0:c}", order.Tax);
            Console.WriteLine("Total: {0:c}", order.Total);
            Console.WriteLine("");
        }
        public static void DisplayState(StateTax state)
        {
            Console.WriteLine("{0} | {1} | {2} ", state.StateAbbreviation, state.StateName, state.TaxRate);
        }
        public static void DisplayProduct(Products products)
        {
            Console.WriteLine("{0} | {1} | {2} ", products.ProductType, products.CostPerSquareFoot, products.LaborCostPerSquareFoot);
        }
        public static void displayProductsPrice(Products products)
        {
            Console.WriteLine("{0} | {1} ", products.ProductType, products.CostPerSquareFoot);
        }

        public static string GetYesNoAnswerFromUser(string prompt)
        {
            while (true)
            {
                Console.Write(prompt + " (Y/N)? ");
                string input = Console.ReadLine().ToUpper();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("You must enter Y/N.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    if (input != "Y" && input != "N")
                    {
                        Console.WriteLine("You must enter Y/N.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue;
                    }
                    return input;
                }
            }
        }
    }
}
