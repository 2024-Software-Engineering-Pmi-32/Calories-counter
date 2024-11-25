using System.Windows;

namespace CaloriesCounter
{
    public partial class SettingsWindow : Window
    {
        private MainProgramWindow mainProgramWindow;

        public SettingsWindow(MainProgramWindow mainWindow)
        {
            InitializeComponent();
            mainProgramWindow = mainWindow;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(this);
            changePasswordWindow.Show();
            this.Hide();
        }

        private void SendFeedback_Click(object sender, RoutedEventArgs e)
        {
            string feedback = FeedbackTextBox.Text;
            string userEmail = "khrystynabroshko@gmail.com";

            UserService userService = new UserService();
            userService.SendFeedback(userEmail, feedback);
            MessageBox.Show("Відгук надіслано!");
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            string faq = "1. Що таке CaloriesCounter?\n" +
                         "Це програма для обліку калорій.\n\n" +
                         "2. Як змінити пароль?\n" +
                         "Перейдіть до налаштувань і натисніть 'Змінити пароль'.\n\n" +
                         "3. Як надіслати відгук?\n" +
                         "Заповніть поле для відгуків і натисніть 'Надіслати відгук'.";
            MessageBox.Show(faq, "Питання та відповіді");
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
