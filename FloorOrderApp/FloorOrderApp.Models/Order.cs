namespace FloorOrderApp.Models
{
    public class Order
    {
        public int OrderNumber;
        public string Name;
        public string StateAbbr;
        public decimal TaxRate;
        public string ProductType;
        public decimal Area;
        public decimal CostPerSquareFoot;
        public decimal LaborCostPerSquareFoot;
        public decimal MaterialCost;
        public decimal LaborCost;
        public decimal TaxCost;
        public decimal TotalCost;
    }
}