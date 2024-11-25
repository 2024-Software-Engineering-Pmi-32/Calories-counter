// <copyright file="MainProgramWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CaloriesCounter
{
    using LiveCharts;
    using LiveCharts.Wpf;
    using Npgsql;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;

    public partial class MainProgramWindow : Window
    {
        public ObservableCollection<int> TotalCalories { get; set; }

        public ObservableCollection<string> Dates { get; set; }

        private readonly int currentUserId;
        private readonly string currentUserName;

        public TimeSpan? WaterReminderTime { get; set; }

        public TimeSpan? MealReminderTime { get; set; }

        public bool WaterReminderEnabled { get; set; }

        public bool MealReminderEnabled { get; set; }

        private readonly Timer reminderTimer;

        public MainProgramWindow(int userId, string userName)
        {
            InitializeComponent();
            this.TotalCalories = new ObservableCollection<int>();
            this.Dates = new ObservableCollection<string>();
            this.DataContext = this;
            this.currentUserId = userId;
            this.currentUserName = userName;
            UserNameTextBlock.Text = userName;

            this.WaterReminderTime = new TimeSpan(8, 0, 0);
            this.MealReminderTime = new TimeSpan(12, 0, 0);
            this.WaterReminderEnabled = true;
            this.MealReminderEnabled = true;

            this.reminderTimer = new Timer(1000);
            this.reminderTimer.Elapsed += this.CheckReminders;
            this.reminderTimer.Start();

            this.LoadFoodIntakes();
            this.UpdateChart();
        }

        private void UpdateChart()
        {
            try
            {
                if (caloriesChart == null)
                {
                    this.ShowFeedback("Графік не ініціалізований.");
                    return;
                }

                caloriesChart.Series.Clear();

                Debug.WriteLine($"TotalCalories Count: {this.TotalCalories.Count}, Dates Count: {this.Dates.Count}");

                if (this.TotalCalories.Count > 0 && this.Dates.Count > 0)
                {
                    caloriesChart.Series.Add(new ColumnSeries
                    {
                        Values = new ChartValues<int>(this.TotalCalories),
                        Title = "Калорії",
                    });

                    if (caloriesChart.AxisX != null && caloriesChart.AxisX.Count > 0)
                    {
                        caloriesChart.AxisX[0].Labels = this.Dates.ToArray();
                    }
                }
                else
                {
                    this.ShowFeedback("Немає даних для відображення на графіку.");
                }
            }
            catch (Exception ex)
            {
                this.ShowFeedback("Помилка при оновленні графіка: " + ex.Message);
            }
        }

        private void ShowFeedback(string message)
        {
            _ = MessageBox.Show(message, "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveGoalButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedGoal = (GoalsComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            this.ShowFeedback($"Ваша ціль збережена: {selectedGoal}");
        }

        private void SaveAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            string allergens = AllergenTextBox.Text;
            this.ShowFeedback($"Ваші алергени збережені: {allergens}");
        }

        private void AddFoodButton_Click(object sender, RoutedEventArgs e)
        {
            FoodIntakeWindow foodIntakeWindow = new FoodIntakeWindow(this, this.currentUserId);
            foodIntakeWindow.Show();
            this.Hide();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Recommendations_Click(object sender, RoutedEventArgs e)
        {
            RecommendationsWindow recommendationsWindow = new RecommendationsWindow(this);
            recommendationsWindow.Show();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(this);
            settingsWindow.Show();
        }

        private void OpenReminderSettings_Click(object sender, RoutedEventArgs e)
        {
            ReminderSettingsWindow settingsWindow = new ReminderSettingsWindow(this);
            settingsWindow.Show();
        }

        private void CheckReminders(object sender, ElapsedEventArgs e)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (this.WaterReminderEnabled && this.WaterReminderTime.HasValue &&
                currentTime.Hours == this.WaterReminderTime.Value.Hours &&
                currentTime.Minutes == this.WaterReminderTime.Value.Minutes)
            {
                this.ShowNotification("Час пити воду!");
            }

            if (this.MealReminderEnabled && this.MealReminderTime.HasValue &&
                currentTime.Hours == this.MealReminderTime.Value.Hours &&
                currentTime.Minutes == this.MealReminderTime.Value.Minutes)
            {
                this.ShowNotification("Час приймати їжу!");
            }
        }

        public void ShowNotification(string message)
        {
            NotificationWindow notificationWindow = new NotificationWindow
            {
                Title = message,
            };
            _ = notificationWindow.ShowDialog();
        }

        private void OpenUsefulArticles_Click(object sender, RoutedEventArgs e)
        {
            UsefulArticlesWindow articlesWindow = new UsefulArticlesWindow();
            articlesWindow.Show();
        }

        private void FoodIntake_Click(object sender, RoutedEventArgs e)
        {
            this.AddFoodButton_Click(sender, e);
        }

        public void AddFoodIntake(int calories, string date)
        {
            if (calories < 0)
            {
                this.ShowFeedback("Калорії не можуть бути від'ємними.");
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                this.TotalCalories.Add(calories);
                this.Dates.Add(date);

                Debug.WriteLine($"Calories Added: {calories}, Date Added: {date}");
                Debug.WriteLine($"TotalCalories Count: {this.TotalCalories.Count}, Dates Count: {this.Dates.Count}");
            });

            this.UpdateChart();
            this.SaveFoodIntakeToDatabase(calories, date);
            this.CheckFoodIntake();
            this.UpdateStatistics();
        }

        private void LoadFoodIntakes()
        {
            string query = "SELECT calories, intake_date FROM food_intake WHERE user_id = @userId ORDER BY intake_date";

            using (var connection = new NpgsqlConnection(this.GetConnectionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    _ = command.Parameters.AddWithValue("userId", this.currentUserId);
                    using (var reader = command.ExecuteReader())
                    {
                        this.TotalCalories.Clear();
                        this.Dates.Clear();

                        while (reader.Read())
                        {
                            int calories = reader.GetInt32(0);
                            DateTime date = reader.GetDateTime(1);

                            this.TotalCalories.Add(calories);
                            this.Dates.Add(date.ToShortDateString());

                            Debug.WriteLine($"Calories Loaded: {calories}, Date Loaded: {date.ToShortDateString()}");
                        }
                    }
                }
            }

            this.UpdateChart();
        }

        private void SaveFoodIntakeToDatabase(int calories, string date)
        {
            using (var connection = new NpgsqlConnection(this.GetConnectionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO food_intake (user_id, calories, date) VALUES (@userId, @calories, @date)", connection))
                {
                    _ = command.Parameters.AddWithValue("userId", this.currentUserId);
                    _ = command.Parameters.AddWithValue("calories", calories);
                    _ = command.Parameters.AddWithValue("date", DateTime.Parse(date));
                    _ = command.ExecuteNonQuery();
                }
            }
        }

        private void CheckFoodIntake()
        {
            int userIdToCheck = this.currentUserId;
            string connectionString = this.GetConnectionString();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM food_intake WHERE user_id = @userId", connection))
                    {
                        _ = command.Parameters.AddWithValue("userId", userIdToCheck);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                this.ShowFeedback("Дані про прийом їжі знайдені для цього користувача.");
                            }
                            else
                            {
                                this.ShowFeedback("Дані про прийом їжі відсутні для цього користувача.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowFeedback($"Помилка бази даних: {ex.Message}");
            }
        }

        public void UpdateStatistics()
        {
            (int totalCalories, int foodIntakeCount) = this.GetStatisticsFromDatabase();

            if (foodIntakeCount > 0)
            {
                this.ShowFeedback($"Загальна кількість калорій: {totalCalories}, Кількість прийомів їжі: {foodIntakeCount}");
            }
            else
            {
                this.ShowFeedback("Немає даних для відображення статистики.");
            }
        }

        private (int totalCalories, int foodIntakeCount) GetStatisticsFromDatabase()
        {
            int totalCalories = 0;
            int foodIntakeCount = 0;
            string connectionString = this.GetConnectionString();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT SUM(calories), COUNT(*) FROM food_intake WHERE user_id = @userId", connection))
                    {
                        _ = command.Parameters.AddWithValue("userId", this.currentUserId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalCalories = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                foodIntakeCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowFeedback($"Помилка бази даних: {ex.Message}");
            }

            return (totalCalories, foodIntakeCount);
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow statisticsWindow = new StatisticsWindow(this, this.currentUserId);
            statisticsWindow.Show();
            this.Hide();
        }

        private void ShowStatistics()
        {
            (int totalCalories, int foodIntakeCount) = this.GetStatisticsFromDatabase();

            if (foodIntakeCount > 0)
            {
                _ = MessageBox.Show(
                    $"Загальна кількість калорій: {totalCalories}, Кількість прийомів їжі: {foodIntakeCount}",
                    "Статистика", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                this.ShowFeedback("Немає даних для відображення статистики.");
            }
        }

        private string GetConnectionString()
        {
            return "Host=localhost;Database=Project111;Username=postgres;Password=your_new_password";
        }
    }
}
