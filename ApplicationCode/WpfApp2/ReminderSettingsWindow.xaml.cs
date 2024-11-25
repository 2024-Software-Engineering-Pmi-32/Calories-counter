using System.Windows;

using System;


namespace CaloriesCounter
{
  
    public partial class ReminderSettingsWindow : Window
    {
        private readonly MainProgramWindow mainProgramWindow;

        public ReminderSettingsWindow(MainProgramWindow mainWindow)
        {
            InitializeComponent();
            this.mainProgramWindow = mainWindow;
            this.LoadReminderSettings();
        }

        private void LoadReminderSettings()
        {
            for (int i = 0; i < 24; i++)
            {
                _ = WaterReminderHourComboBox.Items.Add(i);
                _ = MealReminderHourComboBox.Items.Add(i);
            }

            for (int i = 0; i < 60; i++)
            {
                _ = WaterReminderMinuteComboBox.Items.Add(i);
                _ = MealReminderMinuteComboBox.Items.Add(i);
            }

            if (this.mainProgramWindow.WaterReminderTime.HasValue)
            {
                TimeSpan waterReminderTime = this.mainProgramWindow.WaterReminderTime.Value;
                WaterReminderHourComboBox.SelectedItem = waterReminderTime.Hours;
                WaterReminderMinuteComboBox.SelectedItem = waterReminderTime.Minutes;
            }

            if (this.mainProgramWindow.MealReminderTime.HasValue)
            {
                TimeSpan mealReminderTime = this.mainProgramWindow.MealReminderTime.Value;
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
                this.mainProgramWindow.WaterReminderTime = new TimeSpan(hour, minute, 0);
            }

            if (MealReminderHourComboBox.SelectedItem != null && MealReminderMinuteComboBox.SelectedItem != null)
            {
                int hour = (int)MealReminderHourComboBox.SelectedItem;
                int minute = (int)MealReminderMinuteComboBox.SelectedItem;
                this.mainProgramWindow.MealReminderTime = new TimeSpan(hour, minute, 0);
            }

            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
