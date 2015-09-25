using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.BLL;
using FloorOrderApp.UI.Workflows;

namespace FloorOrderApp.UI
{
    public class MainMenu
    {
        private static OrderManager _manager = new OrderManager();
        private static string _appMode = _manager.GetCurrentMode();

        public void DisplayMenu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("      <->-<->-<->-<->-<->-<->-<->-<->-<->-<->-<->");
                Console.WriteLine("-( Jim & Dan's Superbly Fabulous Flooring Application )-");
                Console.WriteLine("      <->-<->-<->-<->-<->-<->-<->-<->-<->-<->-<->");
                Console.WriteLine("\n(Current Mode: {0})", _appMode);
                Console.WriteLine("\n~~1. Display Order");
                Console.WriteLine("~~2. Add Order");
                Console.WriteLine("~~3. Edit Order");
                Console.WriteLine("~~4. Remove Order");

                if (_appMode == "Production")
                {
                    Console.WriteLine("~~5. Open Order File");
                    Console.WriteLine("~~6. Quit");
                }
                else
                {
                    Console.WriteLine("~~5. Quit");
                }

                Console.Write("\n~~~~~Enter a choice: ");
                string input = Console.ReadLine();
                Console.Clear();

                if (input == "6" || _appMode == "Mock" && input == "5")
                    break;

                ChoiceSelector(input);
            } while (true);

            PurgeMockData();
        }

        private void ChoiceSelector(string input)
        {
            switch (input)
            {
                case "1":
                    DisplayOrdersWorkflow dow = new DisplayOrdersWorkflow();
                    dow.Execute();
                    break;
                case "2":
                    AddOrderWorkflow aow = new AddOrderWorkflow();
                    aow.Execute();
                    break;
                case "3":
                    EditOrderWorkflow eow = new EditOrderWorkflow();
                    eow.Execute();
                    break;
                case "4":
                    RemoveOrderWorkflow row = new RemoveOrderWorkflow();
                    row.Execute();
                    break;
                case "5":
                    OpenOrderFileWorkflow oofw = new OpenOrderFileWorkflow();
                    oofw.Execute();
                    break;
                default:
                    Console.WriteLine("Wrong input. Try again.\n\n");
                    Console.ReadLine();
                    break;
            }
        }

        private void PurgeMockData()
        {
            if (_manager.GetCurrentMode() == "Mock")
                _manager.PurgeMockData();
        }
    }
}
