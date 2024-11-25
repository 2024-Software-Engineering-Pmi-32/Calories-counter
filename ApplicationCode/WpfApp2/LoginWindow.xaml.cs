// <copyright file="LoginWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CaloriesCounter
{
    using Npgsql;
    using System;
    using System.Windows;

    public partial class LoginWindow : Window
    {
        private readonly MainWindow mainWindow;
        private readonly string connectionString = "Host=localhost;Database=Project111;Username=postgres;Password=your_new_password";

        public LoginWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    _ = MessageBox.Show("Успішне підключення до бази даних!");
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Не вдалося підключитись до бази даних: " + ex.Message);
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string password = PasswordBox.Password;

            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT name FROM users WHERE email = @Email AND password = @Password";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        _ = command.Parameters.AddWithValue("@Email", email);
                        _ = command.Parameters.AddWithValue("@Password", password);
                        var userName = command.ExecuteScalar() as string;

                        if (!string.IsNullOrEmpty(userName))
                        {
                            _ = MessageBox.Show("Успішний вхід!");
                            var mainProgramWindow = new MainProgramWindow(1, userName);
                            mainProgramWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            _ = MessageBox.Show("Невірний логін або пароль.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Помилка при вході: " + ex.Message);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow.Show();
            this.Close();
        }

        private void GoToRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(this.mainWindow);
            registerWindow.Show();
            this.Close();
        }

        private void EmailBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmailBox.Text == "Введіть електронну пошту")
            {
                EmailBox.Text = string.Empty;
                EmailBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void EmailBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                EmailBox.Text = "Введіть електронну пошту";
                EmailBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "Введіть пароль")
            {
                PasswordBox.Clear();
                PasswordBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                PasswordBox.Password = "Введіть пароль";
                PasswordBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}
