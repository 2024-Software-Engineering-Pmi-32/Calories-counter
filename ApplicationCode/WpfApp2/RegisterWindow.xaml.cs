using System;
using System.Windows;
using Npgsql;

namespace CaloriesCounter
{
    public partial class RegisterWindow : Window
    {
        private MainWindow mainWindow;
        private string connectionString = "Host=localhost;Database=caloriescounter;Username=postgres;Password=1234 "; 

        public RegisterWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Паролі не співпадають.");
                return;
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Check if the user already exists
                string checkQuery = "SELECT COUNT(1) FROM users WHERE email = @Email";
                using (var checkCommand = new NpgsqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Email", email);
                    int userExists = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (userExists > 0)
                    {
                        MessageBox.Show("Користувач з такою електронною поштою вже існує.");
                        return;
                    }
                }

                // Insert new user
                string insertQuery = "INSERT INTO users (email, password, name, gender) VALUES (@Email, @Password, @Name, @Gender)";
                using (var insertCommand = new NpgsqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Email", email);
                    insertCommand.Parameters.AddWithValue("@Password", password);
                    insertCommand.Parameters.AddWithValue("@Name", email); 
                    insertCommand.Parameters.AddWithValue("@Gender", "not specified"); 
                    insertCommand.ExecuteNonQuery();
                }

            }

            MessageBox.Show("Реєстрація успішна!");

            // Open the main program window and pass the user name or ID if necessary
            var mainUserWindow = new MainProgramWindow(1, email); // Replace '1' with the actual user ID if needed
            mainUserWindow.Show();
            this.Close();
        }

        private void GoToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow(mainWindow);
            loginWindow.Show();
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

        private void ConfirmPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordBox.Password == "Підтвердіть пароль")
            {
                ConfirmPasswordBox.Clear();
                ConfirmPasswordBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void ConfirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                ConfirmPasswordBox.Password = "Підтвердіть пароль";
                ConfirmPasswordBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}
