using LiveCharts;
using LiveCharts.Wpf;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CaloriesCounter
{
    public partial class StatisticsWindow : Window
    {
        private string connectionString = "Host=localhost;Database=Project111;Username=postgres;Password=your_new_password";
        private int currentUserId;
        private MainProgramWindow mainProgramWindow; 

        public StatisticsWindow(MainProgramWindow mainProgramWindow, int userId)
        {
            InitializeComponent();
            this.mainProgramWindow = mainProgramWindow; 
            this.currentUserId = userId; 
        }

        private void ShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Будь ласка, оберіть початкову та кінцеву дати.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var entries = GetFoodEntries(startDate.Value, endDate.Value);

            if (entries.Count == 0)
            {
                MessageBox.Show("Немає даних для відображення у вказаному діапазоні дат.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            double totalCalories = CalculateTotalCalories(entries);

            StatisticsTextBlock.Text = $"Загальні калорії за обраний період: {totalCalories}";

            
            UpdateChart(entries);
        }

        private List<FoodEntry> GetFoodEntries(DateTime startDate, DateTime endDate)
        {
            List<FoodEntry> entries = new List<FoodEntry>();
            string query = "SELECT * FROM food_intake WHERE user_id = @UserId AND intake_date >= @StartDate AND intake_date <= @EndDate"; 

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", currentUserId);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry = new FoodEntry
                            {
                                Calories = Convert.ToDouble(reader["calories"]),
                                Date = Convert.ToDateTime(reader["intake_date"]) 
                            };
                            entries.Add(entry);
                        }
                    }
                }
            }
            return entries;
        }

        private double CalculateTotalCalories(List<FoodEntry> entries)
        {
            return entries.Sum(entry => entry.Calories);
        }

        private void UpdateChart(List<FoodEntry> entries)
        {
            CaloriesChart.Series.Clear();
            var series = new ColumnSeries
            {
                Title = "Калорії",
                Values = new ChartValues<double>(entries.Select(entry => entry.Calories))
            };

            CaloriesChart.Series.Add(series);
            CaloriesChart.AxisX.Clear();
            CaloriesChart.AxisX.Add(new Axis
            {
                Title = "Дата",
                Labels = entries.Select(entry => entry.Date.ToShortDateString()).ToList()
            });
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
            mainProgramWindow.Show(); 
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
