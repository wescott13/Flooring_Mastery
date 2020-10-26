using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.BLL.Helpers;
using Flooring.BLL.Rules;
using Flooring.Models;
using Flooring.Models.Interfaces;
using Flooring.Models.Responses;

namespace Flooring.UI.Workflows
{
    class AddOrderWorkflow
    {
        public void Execute(OrderManager manager)
        {
            Console.Clear();
            Console.WriteLine(ConsoleIO.SeparatorBar);

            DateTime date = DateTime.MinValue;  //no order should be this date.
            decimal area = 0;
            decimal costPerSquareFoot = 0;
            decimal laborCostPerSquareFoot = 0;
            decimal taxRate = 0;
            decimal tax = 0;
            string orderFile = "";
            int orderNumber = 0;
            string orderDate = "";

            while (date == DateTime.MinValue)
                {
                Console.Write("\nEnter date for the order: ");
                string dateInput = Console.ReadLine();

                try
                {
                    DateTime.TryParse(dateInput, out date);
                    string futureDateCheckMessage = AddOrderRules.OrderDate(date);
                    if (futureDateCheckMessage != "")
                    {
                        Console.WriteLine(futureDateCheckMessage);
                        date = DateTime.MinValue;
                    }
                    else
                    {
                        string month = date.ToString("MM");
                        string day = date.ToString("dd");
                        orderDate = "Orders_" + month + day + date.Year + ".txt";
                    }  
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not a valid date.", dateInput);
                    Console.ReadLine();
                }

                OrderDateLookupResponse response = manager.LookupOrderDate(date);
                if (response.Success)
                {
                    if (response.Orders.Count > 0)
                    {
                        List<Order> orders = response.Orders;

                        foreach (Order newOrder in orders)
                        {
                            orderNumber = newOrder.OrderNumber + 1;
                        }
                    }
                }
                else
                {
                    orderNumber = 1;
                }
            }
            
            OrderSettings orderFileSettings = new OrderSettings();
            orderFile = orderFileSettings.SetFilePath(orderDate);

            string customerName = "";
            while (customerName == "")
            {
                Console.Write("\nEnter customer name: ");
                customerName = Console.ReadLine();

                if (customerName == "")
                {
                    Console.WriteLine("Customer name must not be blank.");
                    Console.ReadLine();
                }
                else
                {
                    customerName = "\"" + customerName + "\"";  //Adds escape character
                }
            }
            
            string state = "";
            while (state == "")
            {
                List<string> GetAllStates = manager.GetAllStates();
                foreach (string stateAbbreviation in GetAllStates)
                {
                    Console.WriteLine(stateAbbreviation);
                }
                Console.WriteLine("Enter state:  ");
                state = Console.ReadLine().ToUpper();

                if (state == "")
                {
                    Console.WriteLine("State cannot be blank.");
                }
                else
                {
                    StateTaxesLookupResponse stateResponse = manager.LookupState(state);
                if (stateResponse.Success)
                    {
                        ConsoleIO.DisplayState(stateResponse.stateTax);
                        taxRate = stateResponse.stateTax.TaxRate;
                    }
                else
                    {
                        Console.WriteLine(stateResponse.Message);
                        state = ""; 
                    }
                }
            }
            string product = "";
            while (product == "")
            {
                Dictionary<string, Products> GetAllProducts = manager.GetAllProducts();
                foreach (var products in GetAllProducts)
                {
                    ConsoleIO.displayProductsPrice(products.Value);
                }
            
                Console.WriteLine("Enter product:  ");
                product = Console.ReadLine();

                if (product == "")
                {
                    Console.WriteLine("Product cannot be blank.");
                }
                else
                {
                    ProductLookUpResponse productResponse = manager.LookupProduct(product);
                    if (productResponse.Success)
                    {
                        ConsoleIO.DisplayProduct(productResponse.product);
                        costPerSquareFoot = productResponse.product.CostPerSquareFoot;
                        laborCostPerSquareFoot = productResponse.product.LaborCostPerSquareFoot;
                    }
                    else
                    {
                        Console.WriteLine(productResponse.Message);
                        product = "";
                    }
                }
            }

            while (area == 0)
            {
                Console.Write("\nEnter area for the order (Minimum Order is 100 Square Feet): ");
                string areaInput = Console.ReadLine();

                try
                {
                    decimal.TryParse(areaInput, out area);
                    string areaCheckMessage = AddOrderRules.OrderArea(area);
                    if (areaCheckMessage != "")
                    {
                        Console.WriteLine(areaCheckMessage);
                        area = Convert.ToDecimal(areaInput);
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not a valid area.", areaInput);
                    Console.ReadLine();
                }
            }

            Order order = new Order();
            order.OrderNumber = orderNumber;
            order.Date = date;
            order.CustomerName = customerName;
            order.State = state;
            order.ProductType = product;
            order.MaterialCost = (area * costPerSquareFoot);
            order.LaborCost = (area * laborCostPerSquareFoot);
            tax = Math.Round((order.MaterialCost + order.LaborCost) * (taxRate / 100), MidpointRounding.AwayFromZero);
            order.Tax = tax;
            order.Total = (order.MaterialCost + order.LaborCost + order.Tax);
            order.OrderFile = orderFile;
            order.Area = area;
            order.TaxRate = taxRate;
            order.CostPerSquareFoot = costPerSquareFoot;
            order.LaborCostPerSquareFoot = laborCostPerSquareFoot;

            ConsoleIO.DisplayOrderDetails(order);

            if (ConsoleIO.GetYesNoAnswerFromUser("Add the following information") == "Y")
            {
                OrderResponse addResponse = manager.AddOrder(order);
            }
            else
            {
                Console.WriteLine("Add Cancelled");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

