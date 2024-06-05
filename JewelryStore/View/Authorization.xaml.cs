using JewelryStore.Middleware;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JewelryStore
{
    public partial class Authorization : Window
    {
        private readonly Database.TradeEntities entities;
        private Database.User user;
        private bool isRequireCaptcha;
        private string captchaCode;
        private CaptchaGenerator captchaGenerator;

        public Authorization()
        {
            InitializeComponent();
            entities = new Database.TradeEntities();
            captchaGenerator = new CaptchaGenerator();
        }
        private void OnSignIn(object sender, RoutedEventArgs e)
        {
            if (isRequireCaptcha && captchaCode != null && captchaCode.ToLower() != tbCaptcha.Text.Trim().ToLower())
            {
                MessageBox.Show("Неверная капча");
                return;
            }

            string login = tbLogin.Text.Trim();
            string password = tbPassword.Password.Trim();

            if (login.Length < 1 && password.Length < 1)
            {
                MessageBox.Show("Необходимо ввести логин или пароль");
                return;
            }

            user = entities.Users.Where(u => u.login == login && u.password == password).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Некорректный логин/пароль");
                captchaGenerator.GenerateCaptcha(canvas);
                isRequireCaptcha = true;
                return;
            }

            if (isRequireCaptcha)
            {
                isRequireCaptcha = false;
            }

            switch (user.Role1.name)
            {
                case "Администратор":
                    View.AdminView adminView = new View.AdminView(entities, user);
                    adminView.Owner = this;
                    adminView.Show();
                    Hide();
                    break;
                case "Менеджер":
                    View.ProductView productView = new View.ProductView(entities, user);
                    productView.Owner = this;
                    productView.Show();
                    Hide();
                    break;
            }
        }
    }
}
