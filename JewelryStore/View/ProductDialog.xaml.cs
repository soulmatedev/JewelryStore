using JewelryStore.Database;
using System;
using System.Linq;
using System.Windows;

namespace JewelryStore.View
{
    public partial class ProductDialog : Window
    {
        private readonly TradeEntities _context;

        public Product Product { get; private set; }
        public bool IsReadOnlyArticleNumber { get; set; }

        public ProductDialog(Product product, bool isEditing, TradeEntities context)
        {
            InitializeComponent();
            Product = product;
            IsReadOnlyArticleNumber = isEditing;
            _context = context;

            // Загрузка данных для комбобоксов
            cbCategory.ItemsSource = _context.ProductCategories.ToList();
            cbProvider.ItemsSource = _context.Providers.ToList();
            cbUnit.ItemsSource = _context.Units.ToList();

            if (isEditing)
            {
                tbArticleNumber.Text = Product.articleNumber;
                tbName.Text = Product.name;
                tbDescription.Text = Product.description;
                tbManufacturer.Text = Product.Manufacturer1?.name;
                tbCost.Text = Product.cost.ToString();
                cbCategory.SelectedValue = Product.category;
                cbProvider.SelectedValue = Product.provider;
                cbUnit.SelectedValue = Product.unit;
            }
            else
            {
                tbArticleNumber.Text = GenerateRandomArticleNumber();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Product.articleNumber = tbArticleNumber.Text;
            Product.name = tbName.Text;
            Product.description = tbDescription.Text;

            if (Product.Manufacturer1 == null)
                Product.Manufacturer1 = new Manufacturer();

            Product.Manufacturer1.name = tbManufacturer.Text;

            if (string.IsNullOrWhiteSpace(tbCost.Text))
            {
                MessageBox.Show("Введите стоимость");
                return;
            }

            if (!decimal.TryParse(tbCost.Text, out decimal cost))
            {
                MessageBox.Show("Введите корректное значение стоимости");
                return;
            }

            Product.cost = cost;

            // Установка значений внешних ключей
            Product.category = (int)cbCategory.SelectedValue;
            Product.provider = (int)cbProvider.SelectedValue;
            Product.unit = (int)cbUnit.SelectedValue;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private string GenerateRandomArticleNumber()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }
    }
}
