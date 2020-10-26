using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;

namespace Flooring_Mastery
{
    class Program
    {
        
        static void Main(string[] args)
        {
            OrderManager manager = OrderManagerFactory.Create();
            
            Menu.Start(manager);
        }
    }
}
