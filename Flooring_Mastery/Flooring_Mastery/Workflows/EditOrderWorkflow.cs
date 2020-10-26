using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.BLL.Helpers;
using Flooring.BLL.Rules;
using Flooring.Models;
using Flooring.Models.Responses;

namespace Flooring_Mastery.Workflows
{
    class EditOrderWorkflow
    {
        public void Execute(OrderManager manager)
        {
            DateTime date = DateTime.MinValue;
            decimal taxRate = 0;
            decimal costPerSquareFoot = 0;
            decimal laborCostPerSquareFoot = 0;
            decimal area = 0;
            string setState = "";
            string setProduct = "";

            DisplayOrdersWorkflow displayWorkflow = new DisplayOrdersWorkflow();
            displayWorkflow.Execute(manager);

            DateTime dateObtained = displayWorkflow.Date();
            List<Order> orders = displayWorkflow.Orders();
            int orderInputResult = 0;

            if (dateObtained != DateTime.MinValue)
            {
                Console.WriteLine(ConsoleIO.SeparatorBar);
                Console.WriteLine("Enter the order number to edit.");
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
                    Order order = new Order();
                    order.OrderNumber = result.OrderNumber;
                    order.Date = result.Date;

                        Console.Write("\nEnter customer name: ");
                        string customerName = Console.ReadLine();

                        if (customerName == "")
                        {
                            order.CustomerName = result.CustomerName;
                            
                        }
                        else
                        {
                            customerName = "\"" + customerName + "\"";
                            order.CustomerName = customerName;
                        }

                    string state = "stateResponseError";
                    while (state == "stateResponseError")
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
                        order.State = result.State;
                        order.TaxRate = result.TaxRate;
                        }
                        else
                        {
                            StateTaxesLookupResponse stateResponse = manager.LookupState(state);
                            if (stateResponse.Success)
                            {
                                ConsoleIO.DisplayState(stateResponse.stateTax);
                                taxRate = stateResponse.stateTax.TaxRate;
                                order.TaxRate = taxRate;
                                setState = state;
                            }
                            else
                            {
                                Console.WriteLine(stateResponse.Message);
                                state = "stateResponseError";
                            }
                        }
                    }
                    string product = "product type error";
                    while (product == "product type error")
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
                            order.ProductType = result.ProductType;
                            order.CostPerSquareFoot = result.CostPerSquareFoot;
                            order.LaborCostPerSquareFoot = result.LaborCostPerSquareFoot;
                        }
                        else
                        {
                            ProductLookUpResponse productResponse = manager.LookupProduct(product);
                            if (productResponse.Success)
                            {
                                ConsoleIO.DisplayProduct(productResponse.product);
                                costPerSquareFoot = productResponse.product.CostPerSquareFoot;
                                order.CostPerSquareFoot = costPerSquareFoot;
                                laborCostPerSquareFoot = productResponse.product.LaborCostPerSquareFoot;
                                order.LaborCostPerSquareFoot = laborCostPerSquareFoot;
                                setProduct = product;
                            }
                            else
                            {
                                Console.WriteLine(productResponse.Message);
                                product = "product type error";
                            }
                        }
                    }
                    while (area == 0)
                    {
                        Console.Write("\nEnter area for the order (Minimum Order is 100 Square Feet): ");
                        string areaInput = Console.ReadLine();
                        if (areaInput == "")
                        {
                            order.Area = result.Area;
                            break;
                        }
                        else
                        {
                            try
                            {
                                decimal.TryParse(areaInput, out area);
                                string areaCheckMessage = AddOrderRules.OrderArea(area);
                                if (areaCheckMessage != "")
                                {
                                    Console.WriteLine(areaCheckMessage);
                                    area = 0;
                                }
                                else
                                {
                                    area = Convert.ToDecimal(areaInput);
                                    order.Area = area;
                                }
                                
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("{0} is not a valid area.", areaInput);
                                Console.ReadLine();
                            }
                        }
                    }
                    order.OrderNumber = result.OrderNumber;
                    order.Date = result.Date;
                    order.MaterialCost = order.Area * order.CostPerSquareFoot;
                    order.LaborCost = order.Area * order.LaborCostPerSquareFoot;
                    order.Tax = Math.Round((order.MaterialCost + order.LaborCost) *(order.TaxRate / 100), MidpointRounding.AwayFromZero);
                    order.Total = order.MaterialCost + order.LaborCost + order.Tax;
                    order.OrderFile = result.OrderFile;
                    order.State = setState;
                    order.ProductType = setProduct;
                    
                    OrderSettings orderFileSettings = new OrderSettings();
                    string month = order.Date.ToString("MM");
                    string day = order.Date.ToString("dd");
                    string orderDate = "Orders_" + month + day + order.Date.Year + ".txt";
                    string orderFile = orderFileSettings.SetFilePath(orderDate);
                    order.OrderFile = orderFile;

                    ConsoleIO.DisplayOrderDetails(order);

                    if (ConsoleIO.GetYesNoAnswerFromUser("Edit the following information") == "Y")
                    {
                        OrderResponse addResponse = manager.EditOrder(order);
                    }
                    else
                    {
                        Console.WriteLine("Edit Cancelled");
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
