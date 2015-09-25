using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.BLL;

namespace FloorOrderApp.UI.Workflows
{
    public class OpenOrderFileWorkflow
    {
        public void Execute()
        {
            OrderManager manager = new OrderManager();

            string date = GetDate();
            manager.GetOrderFile(date);
        }

        private string GetDate()
        {
            Validation validation = new Validation();

            string promptUser = "Enter the date the order was placed on (MMDDYYYY)\n\n" +
                                ">>>> ";
            Console.Write(promptUser);
            string input = Console.ReadLine();

            input = validation.NotNull(input, promptUser);
            return validation.IsDateValid(input, promptUser);
        }
    }
}
