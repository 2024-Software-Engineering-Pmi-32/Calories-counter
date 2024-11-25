using System;
using System.Windows;
using System.Windows.Controls;

namespace CaloriesCounter
{
    public partial class RecommendationsWindow : Window
    {
        private MainProgramWindow mainProgramWindow;

        public RecommendationsWindow(MainProgramWindow mainProgramWindow)
        {
            InitializeComponent();
            this.mainProgramWindow = mainProgramWindow;
        }

        private void GetRecommendations_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(WeightTextBox.Text, out double weight) &&
                double.TryParse(HeightTextBox.Text, out double height) &&
                int.TryParse(AgeTextBox.Text, out int age))
            {
                string activityLevel = GetSelectedActivityLevel();
                RecommendationsTextBlock.Text = CalculateRecommendations(weight, height, age, activityLevel);
            }
            else
            {
                RecommendationsTextBlock.Text = "Будь ласка, введіть коректні значення.";
            }
        }

        private string GetSelectedActivityLevel()
        {
            if (ActivityLevelComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                return selectedItem.Content.ToString();
            }
            return "Невідомо";
        }

        private string CalculateRecommendations(double weight, double height, int age, string activityLevel)
        {
            double bmr = 10 * weight + 6.25 * height - 5 * age + 5;
            double tdee;

            switch (activityLevel)
            {
                case "Низький":
                    tdee = bmr * 1.2;
                    break;
                case "Середній":
                    tdee = bmr * 1.55;
                    break;
                case "Високий":
                    tdee = bmr * 1.9;
                    break;
                default:
                    tdee = bmr;
                    break;
            }

            return $"Ваші рекомендації: споживайте приблизно {tdee:F0} калорій на день.";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            mainProgramWindow.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
