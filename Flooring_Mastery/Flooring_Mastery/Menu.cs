using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.BLL.Helpers;
using Flooring.UI.Workflows;
using Flooring_Mastery.Workflows;
using System.Windows;



namespace Flooring_Mastery
{
    public class Menu
    {
        public static void Start(OrderManager manager)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ConsoleIO.SeparatorBar);
                Console.WriteLine("* Flooring Program");
                Console.WriteLine("*");
                Console.WriteLine("* 1. Display Orders");
                Console.WriteLine("* 2. Add an Order");
                Console.WriteLine("* 3. Edit an Order");
                Console.WriteLine("* 4. Remove an Order");
                Console.WriteLine("* 5. Quit");
                Console.WriteLine("*");
                Console.WriteLine(ConsoleIO.SeparatorBar);
                Console.Write("\nEnter selection: ");

                string userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1":
                        DisplayOrdersWorkflow displayWorkflow = new DisplayOrdersWorkflow();
                        displayWorkflow.Execute(manager);
                        break;
                    case "2": 
                        AddOrderWorkflow addWorkflow = new AddOrderWorkflow();
                        addWorkflow.Execute(manager);
                        break;
                    case "3":
                        EditOrderWorkflow editWorkflow = new EditOrderWorkflow();
                        editWorkflow.Execute(manager);
                        break;
                    case "4":
                        RemoveOrderWorkflow removeOrderWorkflow = new RemoveOrderWorkflow();
                        removeOrderWorkflow.Execute(manager);
                        break;
                    case "5":
                        return;
                }

            }

        }
    }
}
