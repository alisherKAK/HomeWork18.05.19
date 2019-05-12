using HomeWork18_05_19.DataAccess;
using HomeWork18_05_19.Models;
using HomeWork18_05_19.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace HomeWork18_05_19.WpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            var login = signInLoginTextBox.Text;
            var password = signInPasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(searchingUser => searchingUser.Login == login);

                if (user != null && DataEncryptor.IsValidPassword(password, user.Password))
                {
                    MessageBox.Show("Добро пожаловать");
                }
                else
                {
                    MessageBox.Show("Пшёл вон");
                }
            }
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            var login = registrationLoginTextBox.Text;
            var password = registrationPasswordBox.Password;
            var repeatPassword = registrationRepeatPasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repeatPassword))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (password != repeatPassword)
            {
                MessageBox.Show("Введенные пароля не совпадают");
                return;
            }

            using (var context = new UserContext())
            {
                if (context.Users.All(user => user.Login != login))
                {
                    User newUser = new User()
                    {
                        Login = login,
                        Password = DataEncryptor.HashPassword(password)
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();

                    MessageBox.Show("Успешная регистрация");
                }
                else
                {
                    MessageBox.Show("Логин занят");
                }
            }
        }
    }
}
