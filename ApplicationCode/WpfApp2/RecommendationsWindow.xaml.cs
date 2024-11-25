// <copyright file="RecommendationsWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CaloriesCounter
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class RecommendationsWindow : Window
    {
        private readonly MainProgramWindow mainProgramWindow;

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
                string activityLevel = this.GetSelectedActivityLevel();
                RecommendationsTextBlock.Text = this.CalculateRecommendations(weight, height, age, activityLevel);
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

        private string GetSelectedGoal()
        {
            if (GoalComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                return selectedItem.Content.ToString();
            }

            return "Підтримка форми";
        }

        private string CalculateRecommendations(double weight, double height, int age, string activityLevel)
        {
            // Обчислення базового метаболізму (BMR) за формулою Харріса-Бенедикта
            double bmr = (10 * weight) + (6.25 * height) - (5 * age) + 5;
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

            string goal = this.GetSelectedGoal();
            string recommendation;

            switch (goal)
            {
                case "Схуднення":
                    recommendation = $"Для схуднення рекомендується споживати приблизно {tdee * 0.8:F0} калорій на день.";
                    break;
                case "Набір ваги":
                    recommendation = $"Для набору ваги рекомендується споживати приблизно {tdee * 1.2:F0} калорій на день.";
                    break;
                case "Підтримка форми":
                default:
                    recommendation = $"Для підтримки форми рекомендується споживати приблизно {tdee:F0} калорій на день.";
                    break;
            }

            return recommendation;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.mainProgramWindow.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
