// <copyright file="ChangePasswordWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CaloriesCounter
{
    using System.Windows;

    public partial class ChangePasswordWindow : Window
    {
        private readonly SettingsWindow settingsWindow;

        public ChangePasswordWindow(SettingsWindow settingsWindow)
        {
            InitializeComponent();
            this.settingsWindow = settingsWindow;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = OldPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (newPassword != confirmPassword)
            {
                _ = MessageBox.Show("Новий пароль і підтвердження не збігаються.");
                return;
            }

            UserService userService = new UserService();
            string username = "user";

            if (userService.ChangePassword(username, oldPassword, newPassword))
            {
                _ = MessageBox.Show("Пароль успішно змінено!");
                this.Close();
                this.settingsWindow.Show();
            }
            else
            {
                _ = MessageBox.Show("Неправильний старий пароль.");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.settingsWindow.Show();
        }
    }
}
