[TestClass]
public class FoodRepositoryTests
{
    private Mock<IDbConnection> _mockConnection;
    private FoodRepository _repository;

    [TestInitialize]
    public void Setup()
    {
        _mockConnection = new Mock<IDbConnection>();
        _repository = new FoodRepository(_mockConnection.Object);
    }

    [TestMethod]
    public void AddFood_ShouldExecuteSQLCommand()
    {
        // Arrange
        var food = new FoodEntry { Name = "Banana", Calories = 89 };
        var mockCommand = new Mock<IDbCommand>();

        _mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
        mockCommand.SetupSet(cmd => cmd.CommandText = It.IsAny<string>());

        // Act
        _repository.AddFood(food);

        // Assert
        mockCommand.VerifySet(cmd => cmd.CommandText = "INSERT INTO Food (Name, Calories) VALUES (@name, @calories)", Times.Once);
        mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
    }
}
[TestClass]
public class FoodRepositoryIntegrationTests
{
    private string _connectionString = "";

    [TestMethod]
    public void AddFood_ShouldInsertDataIntoDatabase()
    {
        // Arrange
        var food = new FoodEntry { Name = "Orange", Calories = 62 };
        var repository = new FoodRepository(new SqlConnection(_connectionString));

        // Act
        repository.AddFood(food);

        // Assert
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT COUNT(*) FROM Food WHERE Name = 'Orange' AND Calories = 62", connection);
            var count = (int)command.ExecuteScalar();
            Assert.AreEqual(1, count);
        }
    }
}
