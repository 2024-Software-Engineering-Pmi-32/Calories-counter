using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CaloriesCounter.DAL
{
    public class FoodServiceAdoNet
    {
        private readonly string _connectionString;

        public FoodServiceAdoNet(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<FoodEntry> GetFoodEntries()
        {
            var foodEntries = new List<FoodEntry>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT Id, Name, Calories, Date FROM FoodEntries";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            foodEntries.Add(new FoodEntry
                            {
                                Name = reader.GetString(1),
                                Calories = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }

            return foodEntries;
        }

        public void AddFoodEntry(FoodEntry entry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO FoodEntries (Name, Calories, Date) VALUES (@Name, @Calories, @Date)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", entry.Name);
                    command.Parameters.AddWithValue("@Calories", entry.Calories);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteFoodEntry(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM FoodEntries WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
