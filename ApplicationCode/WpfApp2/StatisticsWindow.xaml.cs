// <copyright file="StatisticsWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CaloriesCounter
{
    using LiveCharts;
    using LiveCharts.Wpf;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public partial class StatisticsWindow : Window
    {
        private readonly string connectionString = "Host=localhost;Database=Project111;Username=postgres;Password=your_new_password";
        private readonly int currentUserId;
        private readonly MainProgramWindow mainProgramWindow;

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
                _ = MessageBox.Show("Будь ласка, оберіть початкову та кінцеву дати.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var entries = this.GetFoodEntries(startDate.Value, endDate.Value);

            if (entries.Count == 0)
            {
                _ = MessageBox.Show("Немає даних для відображення у вказаному діапазоні дат.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            double totalCalories = this.CalculateTotalCalories(entries);

            StatisticsTextBlock.Text = $"Загальні калорії за обраний період: {totalCalories}";

            this.UpdateChart(entries);
        }

        private List<FoodEntry> GetFoodEntries(DateTime startDate, DateTime endDate)
        {
            List<FoodEntry> entries = new List<FoodEntry>();
            string query = "SELECT * FROM food_intake WHERE user_id = @UserId AND intake_date >= @StartDate AND intake_date <= @EndDate";

            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    _ = command.Parameters.AddWithValue("@UserId", this.currentUserId);
                    _ = command.Parameters.AddWithValue("@StartDate", startDate);
                    _ = command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry = new FoodEntry
                            {
                                Calories = Convert.ToDouble(reader["calories"]),
                                Date = Convert.ToDateTime(reader["intake_date"]),
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
                Values = new ChartValues<double>(entries.Select(entry => entry.Calories)),
            };

            CaloriesChart.Series.Add(series);
            CaloriesChart.AxisX.Clear();
            CaloriesChart.AxisX.Add(new Axis
            {
                Title = "Дата",
                Labels = entries.Select(entry => entry.Date.ToShortDateString()).ToList(),
            });
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.mainProgramWindow.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
