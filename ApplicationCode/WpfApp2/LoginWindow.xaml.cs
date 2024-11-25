using System;
using System.Windows;
using Npgsql;

namespace CaloriesCounter
{
    public partial class LoginWindow : Window
    {
        private MainWindow mainWindow;
        private string connectionString = "Host=localhost;Database=Project111;Username=postgres;Password=your_new_password"; 

        public LoginWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Успішне підключення до бази даних!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не вдалося підключитись до бази даних: " + ex.Message);
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string password = PasswordBox.Password;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT name FROM users WHERE email = @Email AND password = @Password";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        var userName = command.ExecuteScalar() as string;

                        if (!string.IsNullOrEmpty(userName))
                        {
                            MessageBox.Show("Успішний вхід!");
                            var mainProgramWindow = new MainProgramWindow(1, userName); 
                            mainProgramWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Невірний логін або пароль.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при вході: " + ex.Message);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
            this.Close();
        }

        private void GoToRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(mainWindow);
            registerWindow.Show();
            this.Close();
        }

        
        private void EmailBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmailBox.Text == "Введіть електронну пошту")
            {
                EmailBox.Text = "";
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
