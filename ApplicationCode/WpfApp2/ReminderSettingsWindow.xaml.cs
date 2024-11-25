using System;
using System.Windows;

namespace CaloriesCounter
{
    public partial class ReminderSettingsWindow : Window
    {
        private MainProgramWindow mainProgramWindow;

        public ReminderSettingsWindow(MainProgramWindow mainWindow)
        {
            InitializeComponent();
            mainProgramWindow = mainWindow;
            LoadReminderSettings();
        }

        private void LoadReminderSettings()
        {
            for (int i = 0; i < 24; i++)
            {
                WaterReminderHourComboBox.Items.Add(i);
                MealReminderHourComboBox.Items.Add(i);
            }

            for (int i = 0; i < 60; i++)
            {
                WaterReminderMinuteComboBox.Items.Add(i);
                MealReminderMinuteComboBox.Items.Add(i);
            }

            if (mainProgramWindow.WaterReminderTime.HasValue)
            {
                TimeSpan waterReminderTime = mainProgramWindow.WaterReminderTime.Value;
                WaterReminderHourComboBox.SelectedItem = waterReminderTime.Hours;
                WaterReminderMinuteComboBox.SelectedItem = waterReminderTime.Minutes;
            }

            if (mainProgramWindow.MealReminderTime.HasValue)
            {
                TimeSpan mealReminderTime = mainProgramWindow.MealReminderTime.Value;
                MealReminderHourComboBox.SelectedItem = mealReminderTime.Hours;
                MealReminderMinuteComboBox.SelectedItem = mealReminderTime.Minutes;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WaterReminderHourComboBox.SelectedItem != null && WaterReminderMinuteComboBox.SelectedItem != null)
            {
                int hour = (int)WaterReminderHourComboBox.SelectedItem;
                int minute = (int)WaterReminderMinuteComboBox.SelectedItem;
                mainProgramWindow.WaterReminderTime = new TimeSpan(hour, minute, 0);
            }

            if (MealReminderHourComboBox.SelectedItem != null && MealReminderMinuteComboBox.SelectedItem != null)
            {
                int hour = (int)MealReminderHourComboBox.SelectedItem;
                int minute = (int)MealReminderMinuteComboBox.SelectedItem;
                mainProgramWindow.MealReminderTime = new TimeSpan(hour, minute, 0);
            }

            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
