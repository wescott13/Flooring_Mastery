using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.BLL.Helpers
{
    public class OrderSettings
    {
        public string FilePath { get; set; }
        public string SetFilePath(string orderDate)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            string dirOrder = @".\Orders";
            Directory.SetCurrentDirectory(dirOrder);
            string filePath = @".\" + orderDate;
            
            if(!File.Exists(orderDate))
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");
                }
            }
            FilePath = filePath;
            return FilePath;
        }
    }
}
