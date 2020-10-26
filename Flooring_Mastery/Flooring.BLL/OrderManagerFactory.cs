using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Data.Data;

namespace Flooring.BLL
{
    public class OrderManagerFactory
    {
        public static OrderManager Create()
        {
            //.AppSettings is a dictionary
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();
            string FileLocation = ConfigurationManager.AppSettings["FileLocation"].ToString();

            switch (mode)
            {
                case "Test":
                    return new OrderManager(new TestRepository (), new StateTaxesRepository(), new ProductsRepository());
                case "Prod":
                    return new OrderManager(new OrderRepository(FileLocation), new StateTaxesRepository(), new ProductsRepository());
                default:
                    throw new Exception("Mode value in app config is not valid");
            }
        }
    }
}
