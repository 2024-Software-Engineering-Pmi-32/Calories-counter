using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CaloriesCounter
{
    public partial class FoodIntakeWindow : Window
    {
        private MainProgramWindow mainWindow;
        private int userId;

        public FoodIntakeWindow(MainProgramWindow mainWindow, int userId)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.userId = userId;

            
            LoadFoodItems();
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
                new FoodItem("Риба", 206)
                
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
                            command.Parameters.AddWithValue("userId", userId);
                            command.Parameters.AddWithValue("calories", calories);
                            command.Parameters.AddWithValue("intakeDate", date); 
                            command.Parameters.AddWithValue("foodName", foodName); 

                            command.ExecuteNonQuery();
                        }
                    }

                   
                    MessageBox.Show("Food intake added successfully.");
                }
                catch (NpgsqlException ex)
                {
                   
                    MessageBox.Show($"Database error: {ex.Message}");
                }
                catch (Exception ex)
                {
                   
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a food item.");
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Show(); 
        }
    }

    
    public class FoodItem
    {
        public string Name { get; set; }
        public int Calories { get; set; }

        public FoodItem(string name, int calories)
        {
            Name = name;
            Calories = calories;
        }

        public override string ToString()
        {
            return Name; 
        }
    }
}
