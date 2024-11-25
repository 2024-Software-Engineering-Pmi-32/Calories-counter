using System;
using System.Collections.Generic;
using CaloriesCounter.BLL;
using Xunit;

namespace CaloriesCounter.Tests
{
    public class CaloriesCounterTests
    {
        private readonly CaloriesCounter.BLL.CaloriesCounter _caloriesCounter;


        public CaloriesCounterTests()
        {
            _caloriesCounter = new CaloriesCounter.BLL.CaloriesCounter();
        }

        [Fact]
        public void CalculateTotalCalories_ShouldReturnZero_WhenNoEntriesProvided()
        {
            // Arrange
            var foodEntries = new List<FoodEntry>();

            // Act
            var totalCalories = _caloriesCounter.CalculateTotalCalories(foodEntries);

            // Assert
            Assert.Equal(0, totalCalories);
        }

        [Fact]
        public void CalculateTotalCalories_ShouldReturnCorrectTotal_ForSingleEntry()
        {
            // Arrange
            var foodEntries = new List<FoodEntry>
            {
                new FoodEntry { Name = "Apple", Calories = 95 }
            };

            // Act
            var totalCalories = _caloriesCounter.CalculateTotalCalories(foodEntries);

            // Assert
            Assert.Equal(95, totalCalories);
        }

        [Fact]
        public void CalculateTotalCalories_ShouldReturnCorrectTotal_ForMultipleEntries()
        {
            // Arrange
            var foodEntries = new List<FoodEntry>
            {
                new FoodEntry { Name = "Apple", Calories = 95 },
                new FoodEntry { Name = "Banana", Calories = 105 },
                new FoodEntry { Name = "Orange", Calories = 62 }
            };

            // Act
            var totalCalories = _caloriesCounter.CalculateTotalCalories(foodEntries);

            // Assert
            Assert.Equal(262, totalCalories);
        }

        [Fact]
        public void CalculateTotalCalories_ShouldHandleEntriesWithZeroCalories()
        {
            // Arrange
            var foodEntries = new List<FoodEntry>
            {
                new FoodEntry { Name = "Apple", Calories = 95 },
                new FoodEntry { Name = "Water", Calories = 0 }
            };

            // Act
            var totalCalories = _caloriesCounter.CalculateTotalCalories(foodEntries);

            // Assert
            Assert.Equal(95, totalCalories);
        }

        [Fact]
        public void CalculateTotalCalories_ShouldHandleEntriesWithNegativeCalories()
        {
            // Arrange
            var foodEntries = new List<FoodEntry>
            {
                new FoodEntry { Name = "Apple", Calories = 95 },
                new FoodEntry { Name = "ErrorEntry", Calories = -50 }
            };

            // Act
            var totalCalories = _caloriesCounter.CalculateTotalCalories(foodEntries);

            // Assert
            Assert.Equal(45, totalCalories);
        }
    }
}
