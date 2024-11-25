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

namespace CaloriesCounter
{
    public partial class MainProgramWindow : Window
    {
        public ObservableCollection<int> TotalCalories { get; set; }
        public ObservableCollection<string> Dates { get; set; }
        private int currentUserId;
        private string currentUserName;
        public TimeSpan? WaterReminderTime { get; set; }
        public TimeSpan? MealReminderTime { get; set; }
        public bool WaterReminderEnabled { get; set; }
        public bool MealReminderEnabled { get; set; }
        private Timer reminderTimer;

        public MainProgramWindow(int userId, string userName)
        {
            InitializeComponent();
            TotalCalories = new ObservableCollection<int>();
            Dates = new ObservableCollection<string>();
            DataContext = this;
            currentUserId = userId;
            currentUserName = userName;
            UserNameTextBlock.Text = userName;

            
            WaterReminderTime = new TimeSpan(8, 0, 0);
            MealReminderTime = new TimeSpan(12, 0, 0);
            WaterReminderEnabled = true;
            MealReminderEnabled = true;

            
            reminderTimer = new Timer(1000);
            reminderTimer.Elapsed += CheckReminders;
            reminderTimer.Start();

            LoadFoodIntakes(); 
            UpdateChart(); 
        }


        private void UpdateChart()
        {
            try
            {
                if (caloriesChart == null)
                {
                    ShowFeedback("Графік не ініціалізований.");
                    return;
                }

                caloriesChart.Series.Clear();

              
                Debug.WriteLine($"TotalCalories Count: {TotalCalories.Count}, Dates Count: {Dates.Count}");

                if (TotalCalories.Count > 0 && Dates.Count > 0)
                {
                    caloriesChart.Series.Add(new ColumnSeries
                    {
                        Values = new ChartValues<int>(TotalCalories),
                        Title = "Калорії"
                    });

                    if (caloriesChart.AxisX != null && caloriesChart.AxisX.Count > 0)
                    {
                        caloriesChart.AxisX[0].Labels = Dates.ToArray();
                    }
                }
                else
                {
                    ShowFeedback("Немає даних для відображення на графіку.");
                }
            }
            catch (Exception ex)
            {
                ShowFeedback("Помилка при оновленні графіка: " + ex.Message);
            }
        }


        private void ShowFeedback(string message)
        {
            MessageBox.Show(message, "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveGoalButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedGoal = (GoalsComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            ShowFeedback($"Ваша ціль збережена: {selectedGoal}");
        }

        private void SaveAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            string allergens = AllergenTextBox.Text;
            ShowFeedback($"Ваші алергени збережені: {allergens}");
        }

        private void AddFoodButton_Click(object sender, RoutedEventArgs e)
        {
            FoodIntakeWindow foodIntakeWindow = new FoodIntakeWindow(this, currentUserId);
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

            if (WaterReminderEnabled && WaterReminderTime.HasValue &&
                currentTime.Hours == WaterReminderTime.Value.Hours &&
                currentTime.Minutes == WaterReminderTime.Value.Minutes)
            {
                ShowNotification("Час пити воду!");
            }

            if (MealReminderEnabled && MealReminderTime.HasValue &&
                currentTime.Hours == MealReminderTime.Value.Hours &&
                currentTime.Minutes == MealReminderTime.Value.Minutes)
            {
                ShowNotification("Час приймати їжу!");
            }
        }

        public void ShowNotification(string message)
        {
            NotificationWindow notificationWindow = new NotificationWindow();
            notificationWindow.Title = message;
            notificationWindow.ShowDialog();
        }

        private void OpenUsefulArticles_Click(object sender, RoutedEventArgs e)
        {
            UsefulArticlesWindow articlesWindow = new UsefulArticlesWindow();
            articlesWindow.Show();
        }

        private void FoodIntake_Click(object sender, RoutedEventArgs e)
        {
            AddFoodButton_Click(sender, e);
        }

        public void AddFoodIntake(int calories, string date)
        {
            if (calories < 0)
            {
                ShowFeedback("Калорії не можуть бути від'ємними.");
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                TotalCalories.Add(calories);
                Dates.Add(date);

              
                Debug.WriteLine($"Calories Added: {calories}, Date Added: {date}");
                Debug.WriteLine($"TotalCalories Count: {TotalCalories.Count}, Dates Count: {Dates.Count}");
            });

           
            UpdateChart();
            SaveFoodIntakeToDatabase(calories, date);
            CheckFoodIntake();
            UpdateStatistics(); 
        }
        private void LoadFoodIntakes()
        {
            string query = "SELECT calories, intake_date FROM food_intake WHERE user_id = @userId ORDER BY intake_date";

            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("userId", currentUserId);
                    using (var reader = command.ExecuteReader())
                    {
                        TotalCalories.Clear();
                        Dates.Clear();

                        while (reader.Read())
                        {
                            int calories = reader.GetInt32(0);
                            DateTime date = reader.GetDateTime(1);

                            TotalCalories.Add(calories);
                            Dates.Add(date.ToShortDateString());

                           
                            Debug.WriteLine($"Calories Loaded: {calories}, Date Loaded: {date.ToShortDateString()}");
                        }
                    }
                }
            }
            UpdateChart(); 
        }


        private void SaveFoodIntakeToDatabase(int calories, string date)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO food_intake (user_id, calories, date) VALUES (@userId, @calories, @date)", connection))
                {
                    command.Parameters.AddWithValue("userId", currentUserId);
                    command.Parameters.AddWithValue("calories", calories);
                    command.Parameters.AddWithValue("date", DateTime.Parse(date));
                    command.ExecuteNonQuery();
                }
            }
        }

        private void CheckFoodIntake()
        {
            int userIdToCheck = currentUserId;
            string connectionString = GetConnectionString();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM food_intake WHERE user_id = @userId", connection))
                    {
                        command.Parameters.AddWithValue("userId", userIdToCheck);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                ShowFeedback("Дані про прийом їжі знайдені для цього користувача.");
                            }
                            else
                            {
                                ShowFeedback("Дані про прийом їжі відсутні для цього користувача.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowFeedback($"Помилка бази даних: {ex.Message}");
            }
        }

        public void UpdateStatistics()
        {
            (int totalCalories, int foodIntakeCount) = GetStatisticsFromDatabase();

            if (foodIntakeCount > 0)
            {
                ShowFeedback($"Загальна кількість калорій: {totalCalories}, Кількість прийомів їжі: {foodIntakeCount}");
            }
            else
            {
                ShowFeedback("Немає даних для відображення статистики.");
            }
        }

        private (int totalCalories, int foodIntakeCount) GetStatisticsFromDatabase()
        {
            int totalCalories = 0;
            int foodIntakeCount = 0;
            string connectionString = GetConnectionString();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT SUM(calories), COUNT(*) FROM food_intake WHERE user_id = @userId", connection))
                    {
                        command.Parameters.AddWithValue("userId", currentUserId);
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
                ShowFeedback($"Помилка бази даних: {ex.Message}");
            }

            return (totalCalories, foodIntakeCount);
        }


        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow statisticsWindow = new StatisticsWindow(this, currentUserId); 
            statisticsWindow.Show();
            this.Hide(); 
        }


        private void ShowStatistics()
        {
            (int totalCalories, int foodIntakeCount) = GetStatisticsFromDatabase();

            if (foodIntakeCount > 0)
            {
                MessageBox.Show($"Загальна кількість калорій: {totalCalories}, Кількість прийомів їжі: {foodIntakeCount}",
                                "Статистика", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ShowFeedback("Немає даних для відображення статистики.");
            }
        }

        private string GetConnectionString()
        {
            return "Host=localhost;Database=Project111;Username=postgres;Password=your_new_password";
        }
    }
}
