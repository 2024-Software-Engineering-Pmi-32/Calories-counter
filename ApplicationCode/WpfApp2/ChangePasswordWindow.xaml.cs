using System.Windows;

namespace CaloriesCounter
{
    public partial class ChangePasswordWindow : Window
    {
        private SettingsWindow settingsWindow;

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
                MessageBox.Show("Новий пароль і підтвердження не збігаються.");
                return;
            }

            UserService userService = new UserService();
            string username = "user";

            if (userService.ChangePassword(username, oldPassword, newPassword))
            {
                MessageBox.Show("Пароль успішно змінено!");
                this.Close();
                settingsWindow.Show();
            }
            else
            {
                MessageBox.Show("Неправильний старий пароль.");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            settingsWindow.Show();
        }
    }
}
