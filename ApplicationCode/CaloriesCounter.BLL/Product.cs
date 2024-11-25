using System.Collections.Generic;

namespace CaloriesCounter
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
        public bool IsDish { get; set; } = false;
        public bool IsDrink { get; set; } = false;
        public List<Product> Ingredients { get; set; } = new List<Product>();
    }
}
