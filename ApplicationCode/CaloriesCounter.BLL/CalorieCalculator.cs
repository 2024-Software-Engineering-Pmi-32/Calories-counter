using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaloriesCounter.DAL;

namespace CaloriesCounter.BLL
{
    public class CalorieCalculator
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

