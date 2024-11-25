namespace WpfAppCaloriesCounter.BLL
{
	public class CalorieCalculator
	{
		public int CalculateTotalCalories(List<FoodEntry> foodEntries)
		{
			int totalCalories = 0;
			foreach (var entry in foodEntries)
			{
				totalCalories += entry.Calories;
			}
			return totalCalories;
		}
	}
}
