using Xunit;
using System.Collections.Generic;

namespace WpfAppCaloriesCounter.BLL.Tests
{
    public class CalorieCalculatorTests
    {
        [Fact]
        public void CalculateTotalCalories_ShouldReturnCorrectSum()
        {
            
            var calculator = new CalorieCalculator();
            var foodEntries = new List<FoodEntry>
            {
                new FoodEntry { Calories = 200 },
                new FoodEntry { Calories = 150 },
                new FoodEntry { Calories = 100 }
            };

            
            var result = calculator.CalculateTotalCalories(foodEntries);

            
            Assert.Equal(450, result);
        }

        [Fact]
        public void CalculateTotalCalories_WithEmptyList_ShouldReturnZero()
        {
            
            var calculator = new CalorieCalculator();
            var foodEntries = new List<FoodEntry>();

            
            var result = calculator.CalculateTotalCalories(foodEntries);

            
            Assert.Equal(0, result);
        }
    }

    public class FoodServiceTests
    {
        [Fact]
        public void AddFoodEntry_ShouldIncreaseListCount()
        {
            
            var service = new FoodService();
            var initialCount = service.GetAllEntries().Count;
            var foodEntry = new FoodEntry { Name = "Apple", Calories = 95 };

            
            service.AddFoodEntry(foodEntry);
            var resultCount = service.GetAllEntries().Count;

            
            Assert.Equal(initialCount + 1, resultCount);
        }

        [Fact]
        public void GetAllEntries_ShouldReturnCorrectEntries()
        {
            
            var service = new FoodService();
            var foodEntry1 = new FoodEntry { Name = "Banana", Calories = 105 };
            var foodEntry2 = new FoodEntry { Name = "Orange", Calories = 65 };
            service.AddFoodEntry(foodEntry1);
            service.AddFoodEntry(foodEntry2);

            
            var entries = service.GetAllEntries();

            
            Assert.Contains(foodEntry1, entries);
            Assert.Contains(foodEntry2, entries);
        }
    }
    [TestClass]
    public class FoodServiceTests
    {
        private Mock<IFoodRepository> _mockRepo;
        private FoodService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IFoodRepository>();
            _service = new FoodService(_mockRepo.Object);
        }

        [TestMethod]
        public void SaveFood_ShouldThrowException_WhenFoodIsInvalid()
        {
            // Arrange
            var food = new FoodEntry { Name = "", Calories = 0 };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => _service.SaveFood(food));
        }

        [TestMethod]
        public void SaveFood_ShouldCallDAL_WhenFoodIsValid()
        {
            // Arrange
            var food = new FoodEntry { Name = "Apple", Calories = 50 };

            // Act
            _service.SaveFood(food);

            // Assert
            _mockRepo.Verify(repo => repo.AddFood(food), Times.Once);
        }
    }

}
