using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Nikolay_YW.HockeyDB;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nikolay_YW
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;
            var username = LoginBox.Text;
            var IsFind = false;

            if (!(string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password)))
            {
                using (DataContext context = new DataContext())
                {
                    foreach (var item in context.Accounts)
                    {
                        if (item.Password == password && item.Usrename == username)
                        {
                            IsFind = true;
                        }
                    }
                }

                if (IsFind)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверные данные для входа.\nОбратитесь к своему системному администратору ","Отказано в доступе");
                }
            }
            else
            {
                MessageBox.Show("Поле логина и пароля не могут быть пустыми", "Ошибка!");
            }
        }
    }
}
