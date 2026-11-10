using Kashapov41.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kashapov41
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        string fullCaptcha;
        public AuthPage()
        {
            InitializeComponent();
            UpdateCaptcha();
        }

        private void UpdateCaptcha()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char c1 = chars[random.Next(chars.Length)];
            char c2 = chars[random.Next(chars.Length)];
            char c3 = chars[random.Next(chars.Length)];
            char c4 = chars[random.Next(chars.Length)];
            fullCaptcha = c1.ToString() + c2.ToString() + c3.ToString() + c4.ToString();
            Captcha1.Text = c1.ToString();
            Captcha2.Text = c2.ToString();
            Captcha3.Text = c3.ToString();
            Captcha4.Text = c4.ToString();
            CaptchaTB.Text = "";
        }
        private async void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string password = PasswordTB.Text;
            if (CaptchaTB.Text == fullCaptcha || CaptchaTB.Visibility == Visibility.Hidden)
            {
                UpdateCaptcha();
                if (login == "" || password == "")
                {
                    MessageBox.Show("Ошибка. Имеются пустые поля.");
                    return;
                }
                User user = Kashapov41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
                if (user != null)
                {
                    Manager.MainFrame.Navigate(new ProductPage(user));
                    CaptchaSP.Visibility = Visibility.Hidden;
                    CaptchaTB.Visibility = Visibility.Hidden;
                    LoginTB.Text = "";
                    PasswordTB.Text = "";
                }
                else
                {
                    MessageBox.Show("Ошибка. Введены неправильные логин или пароль.");
                    CaptchaSP.Visibility = Visibility.Visible;
                    CaptchaTB.Visibility = Visibility.Visible;
                }
            }
            else if (CaptchaTB.Text != "" && CaptchaSP.Visibility == Visibility.Visible)
            {
                UpdateCaptcha();
                MessageBox.Show("Ошибка. Проверка не пройдена.");
                EnterBtn.IsEnabled = false;
                await Task.Delay(10000);
                EnterBtn.IsEnabled = true;
            }
            else
            {
                UpdateCaptcha();
                MessageBox.Show("Ошибка. Имеются пустые поля.");
                return;
            }
        }

        private void EnterAsGuestBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateCaptcha();
            Manager.MainFrame.Navigate(new ProductPage(null));
        }
    }
}
