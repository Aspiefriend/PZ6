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
using PZ6.Models;
using PZ6.Services;
using System.Data.Entity;
using System.Reflection;

namespace PZ6.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        private int click;
        public Autho()
        {
            InitializeComponent();
            click = 0;
        }
        private void GenerateCaptcha()
        {
            tblCaptcha.Visibility = Visibility.Visible;
            tbCaptcha.Visibility = Visibility.Visible;

            string captchaText = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.Text = captchaText;
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }
        private void ClearInput()
        {
            txtPassword.Password = "";
        }
        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null,null));
        }
        private List<string> GetRoleNames(Accounts account)
        {

            using (var db = Helper.GetContext())
            {
                var roleNames = db.AccountRoles.Where(ar => ar.id_account == account.id_account).Join(db.Roles,
                    ar => ar.id_role,
                    r => r.id_role,
                    (ar, r) => r.role_name).ToList();
                return roleNames;
            }  


        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();
            string passwordHash = Hash.HashPasswords(password);
            click += 1;

            try
            {
                using (var db = Helper.GetContext())
                {

                    var account = db.Accounts.Where(a => a.login == login && a.password_hash == passwordHash).FirstOrDefault();

                    if(click == 1) 
                    {
                        if (account != null)
                        {
                            var roleNames = GetRoleNames(account);

                            if (roleNames.Count == 1)
                            {
                                LoadPage(account, roleNames[0]);
                            }
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("Войти как сотрудник?", "Выбор роли", MessageBoxButton.YesNo);

                                switch (result)
                                {
                                    case MessageBoxResult.Yes:
                                        LoadPage(account, roleNames.Where(rn => rn == "Сотрудник").FirstOrDefault());
                                        break;
                                    case MessageBoxResult.No:
                                        LoadPage(account, roleNames.Where(rn => rn == "Клиент").FirstOrDefault());
                                        break;

                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Вы ввели логин или пароль неверно. Придётся пройти капчу");
                            GenerateCaptcha();
                            ClearInput();
                        }
                    }
                    else
                    {

                        if(account != null && tblCaptcha.Text == tbCaptcha.Text)
                        {
                            var roleNames = GetRoleNames(account);

                            if (roleNames.Count == 1)
                            {
                                LoadPage(account, roleNames[0]);
                            }
                            else
                            {
                                MessageBoxResult result = MessageBox.Show("Войти как сотрудник?", "Выбор роли", MessageBoxButton.YesNo);

                                switch (result)
                                {
                                    case MessageBoxResult.Yes:
                                        LoadPage(account, roleNames.Where(rn => rn == "Сотрудник").FirstOrDefault());
                                        break;
                                    case MessageBoxResult.No:
                                        LoadPage(account, roleNames.Where(rn => rn == "Клиент").FirstOrDefault());
                                        break;

                                }
                            }

                        }
                        else {
                            MessageBox.Show("Вы ввели неправильно или логин, или пароль, или капчу. Повторите ");
                            ClearInput();
                        
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в работе с Базой Данных. {ex.Message}");

            }

            
        }

        private void LoadPage(Accounts account, string role)
        {
            click = 0;
            NavigationService.Navigate(new Client(account, role));


        }
    }
}
