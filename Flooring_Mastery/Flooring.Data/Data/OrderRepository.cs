using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;
using Flooring.Models.Interfaces;

namespace Flooring.Data.Data
{
    public class OrderRepository: IOrderRepository
    {
        private string _filePath = "";
        Dictionary<string, Order> orders;
        private DirectoryInfo _dir;

        public OrderRepository(string filePath)
        {
            orders = new Dictionary<string, Order>();
            _filePath = filePath;
            _dir = new DirectoryInfo(@_filePath);

            populateOrdersFromFile();
        }
        public List<Order> List()
        {
            List<Order> dayOrders = new List<Order>();

            using (StreamReader sr = new StreamReader(_filePath))
            {
                sr.ReadLine();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    Order order = new Order();
                    string[] columns = line.Split(',');

                    order.OrderNumber = int.Parse(columns[0]);
                    order.CustomerName = columns[1];
                    order.State = columns[2];
                    order.TaxRate = decimal.Parse(columns[3]);
                    order.ProductType = columns[4];
                    order.Area = decimal.Parse(columns[5]);
                    order.CostPerSquareFoot = decimal.Parse(columns[6]);
                    order.LaborCostPerSquareFoot = decimal.Parse(columns[7]);
                    order.MaterialCost = decimal.Parse(columns[8]);
                    order.LaborCost = decimal.Parse(columns[9]);
                    order.Tax = decimal.Parse(columns[10]);
                    order.Total = decimal.Parse(columns[11]);

                    dayOrders.Add(order);
                }
            }
            return dayOrders;
        }

        public void SaveOrder(Order order)
        {
            using (StreamWriter sw = new StreamWriter(order.OrderFile, true))  //A method can call other methods
            {
                string line = CreateOrderForOrderDay(order);
                sw.WriteLine(line);
            }
            orders.Clear();
            populateOrdersFromFile();

        }
          private string CreateOrderForOrderDay(Order order)
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                order.OrderNumber, order.CustomerName, order.State, order.TaxRate,
                order.ProductType, order.Area, order.CostPerSquareFoot,
                order.LaborCostPerSquareFoot, order.MaterialCost, order.LaborCost, order.Tax, order.Total);
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
           
            FileInfo[] orderFiles = _dir.GetFiles("Orders_" + date + ".txt");
            FileInfo file = orderFiles[0];

            string line = null;

            string line_to_edit = order.OrderNumber.ToString();

            using (StreamReader reader = new StreamReader(file.FullName))
            {
                using (StreamWriter writer = new StreamWriter(file.FullName + ".temp"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith(line_to_edit))
                        {
                            line = CreateOrderForOrderDay(order);
                            writer.WriteLine(line);
                            continue;
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }  
                    }
                }
            }
            if (File.Exists(file.FullName + ".temp"))
            {
                File.Delete(file.FullName);
                File.Move(file.FullName + ".temp", file.FullName);
            }

            orders.Clear();
            populateOrdersFromFile();
        }

        public void RemoveOrder(Order order)
        {
            string date = order.Date.ToString("MMddyyyy");
            orders.Remove(date + order.OrderNumber);
            
            FileInfo[] orderFiles = _dir.GetFiles("Orders_" + date + ".txt");
            FileInfo file = orderFiles[0];
            
            string line = null;

            string line_to_delete = order.OrderNumber.ToString();

            using (StreamReader reader = new StreamReader(file.FullName))
            {
                using (StreamWriter writer = new StreamWriter(file.FullName + ".temp"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith(line_to_delete))
                            continue;

                        writer.WriteLine(line);
                    }
                }
            }
            if (File.Exists(file.FullName + ".temp"))
            {
                File.Delete(file.FullName);
                File.Move(file.FullName + ".temp", file.FullName);
            }

            orders.Clear();
            populateOrdersFromFile();
        }

        private void populateOrdersFromFile()
        {
            FileInfo[] orderFiles = _dir.GetFiles("*.txt", SearchOption.AllDirectories); //* wildcard starts with anything, ends with .txt
            
            foreach (FileInfo file in orderFiles)
            {
                string fileName = file.Name;  //split and drop
                string[] fileDateSplit = fileName.Split('_', '.');
                string fileDate = fileDateSplit[1];
                string pattern = "MMddyyyy";
                DateTime parsedDate;
                DateTime.TryParseExact(fileDate, pattern, null, DateTimeStyles.None, out parsedDate);

                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    sr.ReadLine();
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Order order = new Order();
                        string[] columns = line.Split(',');

                        order.OrderNumber = int.Parse(columns[0]);
                        order.CustomerName = columns[1];
                        order.State = columns[2];
                        order.TaxRate = decimal.Parse(columns[3]);
                        order.ProductType = columns[4];
                        order.Area = decimal.Parse(columns[5]);
                        order.CostPerSquareFoot = decimal.Parse(columns[6]);
                        order.LaborCostPerSquareFoot = decimal.Parse(columns[7]);
                        order.MaterialCost = decimal.Parse(columns[8]);
                        order.LaborCost = decimal.Parse(columns[9]);
                        order.Tax = decimal.Parse(columns[10]);
                        order.Total = decimal.Parse(columns[11]);
                        order.Date = parsedDate;

                        orders.Add(fileDate + order.OrderNumber, order);  //customkey
                    }
                }
            }
        }
    }
}
