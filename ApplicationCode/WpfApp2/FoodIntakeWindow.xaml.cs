// <copyright file="FoodIntakeWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CaloriesCounter
{
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public partial class FoodIntakeWindow : Window
    {
        private readonly MainProgramWindow mainWindow;
        private readonly int userId;

        public FoodIntakeWindow(MainProgramWindow mainWindow, int userId)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.userId = userId;

            this.LoadFoodItems();
        }

        private void LoadFoodItems()
        {
            List<FoodItem> foodItems = new List<FoodItem>
            {
                new FoodItem("Яблуко", 52),
                new FoodItem("Банан", 89),
                new FoodItem("Апельсин", 47),
                new FoodItem("Хліб", 265),
                new FoodItem("Молоко", 42),
                new FoodItem("Яйце", 155),
                new FoodItem("Куряче філе", 165),
                new FoodItem("Риба", 206),
            };

            FoodItemsComboBox.ItemsSource = foodItems;
            FoodItemsComboBox.DisplayMemberPath = "Name";
            FoodItemsComboBox.SelectedValuePath = "Calories";
        }

        private void AddFoodButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = FoodItemsComboBox.SelectedItem as FoodItem;

            if (selectedItem != null)
            {
                int calories = selectedItem.Calories;
                DateTime date = DateTime.Now;
                string foodName = selectedItem.Name;

                string connectionString = "Host=localhost;Database=Project111;Username=postgres;Password=your_new_password";

                try
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var command = new NpgsqlCommand("INSERT INTO food_intake (user_id, calories, intake_date, food_name) VALUES (@userId, @calories, @intakeDate, @foodName)", connection))
                        {
                            _ = command.Parameters.AddWithValue("userId", this.userId);
                            _ = command.Parameters.AddWithValue("calories", calories);
                            _ = command.Parameters.AddWithValue("intakeDate", date);
                            _ = command.Parameters.AddWithValue("foodName", foodName);

                            _ = command.ExecuteNonQuery();
                        }
                    }

                    _ = MessageBox.Show("Food intake added successfully.");
                }
                catch (NpgsqlException ex)
                {
                    _ = MessageBox.Show($"Database error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                _ = MessageBox.Show("Please select a food item.");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.mainWindow.Show();
        }
    }

    public class FoodItem
    {
        public string Name { get; set; }

        public int Calories { get; set; }

        public FoodItem(string name, int calories)
        {
            this.Name = name;
            this.Calories = calories;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
