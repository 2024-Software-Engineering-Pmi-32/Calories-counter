using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CaloriesCounter.BLL
{
    public class FoodEntry
    {
        public string Name { get; set; }
        public double Calories { get; set; }
    }
    public class CaloriesCounter
    {
        public double CalculateTotalCalories(List<FoodEntry> foodEntries)
        {
            double totalCalories = 0;
            foreach (var entry in foodEntries)
            {
                totalCalories += entry.Calories;
            }
            return totalCalories;
        }
    }
}
