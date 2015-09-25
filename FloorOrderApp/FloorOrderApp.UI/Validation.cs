using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.BLL;
using FloorOrderApp.Models;

namespace FloorOrderApp.UI
{
    public class Validation
    {
        public decimal IsDecimalValid(string input, string promptUser)
        {
            do
            {
                decimal dInput;

                if (decimal.TryParse(input, out dInput) && dInput > 0)
                    return dInput;

                Console.Clear();
                Console.WriteLine("That was not a valid decimal value.\n");
                Console.Write(promptUser);
                input = Console.ReadLine();
            } while (true);
        }

        public Tax IsStateValid(string input, string promptUser)
        {
            OrderManager manager = new OrderManager();
            var states = manager.GetStates();
            var stateAbbrvs = states.Select(st => st.StateAbbr).ToList();

            do
            {
                input = NotNull(input, promptUser);
                input = input.ToUpper();

                if (stateAbbrvs.Contains(input))
                {
                    var input1 = input;
                    var tax = states.Where(mat => mat.StateAbbr == input1).ToList();

                    return tax[0];
                }

                Console.Clear();
                Console.WriteLine("That was not an avaiable location.\n");
                Console.Write(promptUser);
                input = Console.ReadLine();
            } while (true);
        }

        public Product IsProductTypeValid(string input, string promptUser)
        {
            OrderManager manager = new OrderManager();
            var materials = manager.GetMaterials();
            var products = materials.Select(mat => mat.ProductType).ToList();

            do
            {
                input = NotNull(input, promptUser);
                input = input.Substring(0, 1).ToUpper() + input.Substring(1, input.Length - 1).ToLower();

                if (products.Contains(input))
                {
                    var input1 = input;
                    var product = materials.Where(mat => mat.ProductType == input1).ToList();

                    return product[0];
                }

                Console.Clear();
                Console.WriteLine("That material is not available.\n");
                Console.Write(promptUser);
                input = Console.ReadLine();
            } while (true);
        }

        public int IsIntegerValid(string input, string promptUser)
        {
            do
            {
                int price;

                if (int.TryParse(input, out price))
                    return price;

                Console.Clear();
                Console.WriteLine("That was not a valid area.\n");
                Console.Write(promptUser);
                input = Console.ReadLine();
            } while (true);
        }

        public string IsDateValid(string input, string promptUser)
        {
            do
            {
                DateTime temp;

                if (DateTime.TryParseExact(input, "MMddyyyy", new CultureInfo("en-US"), DateTimeStyles.AdjustToUniversal, out temp))
                    return input;

                Console.Clear();
                Console.WriteLine("Date was in the wrong format.\n");
                Console.Write(promptUser);
                input = Console.ReadLine();
            } while (true);
        }

        public string NotNull(string input, string promptUser)
        {
            do
            {
                if (input != null)
                    return input;

                Console.Clear();
                Console.WriteLine("No input detected.\n");
                Console.Write(promptUser);
                input = Console.ReadLine();
            } while (true);
        }
    }
}
