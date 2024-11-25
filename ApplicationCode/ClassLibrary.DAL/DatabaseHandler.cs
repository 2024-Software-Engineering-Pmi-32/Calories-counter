using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleApp.DAL
{
    public class DatabaseHandler
    {
        private readonly string _connectionString;

        public DatabaseHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<FoodEntry> GetFoodEntries()
        {
            var foodEntries = new List<FoodEntry>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM FoodEntries", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        foodEntries.Add(new FoodEntry
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Calories = reader.GetDouble(2)
                        });
                    }
                }
            }

            return foodEntries;
        }

        public bool InsertFoodEntry(FoodEntry entry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "INSERT INTO FoodEntries (Name, Calories) VALUES (@Name, @Calories)", connection);
                command.Parameters.AddWithValue("@Name", entry.Name);
                command.Parameters.AddWithValue("@Calories", entry.Calories);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }

    public class FoodEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }
    }
}
