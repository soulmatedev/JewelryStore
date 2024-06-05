using JewelryStore.Database;
using System.Windows;

namespace JewelryStore.View
{
    public partial class ProductDialog : Window
    {
        public Product Product { get; private set; }

        public ProductDialog(Product product)
        {
            InitializeComponent();
            Product = product;

            tbName.Text = Product.name;
            tbDescription.Text = Product.description;
            tbManufacturer.Text = Product.Manufacturer1?.name;
            tbCost.Text = Product.cost.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Product.name = tbName.Text;
            Product.description = tbDescription.Text;

            // Обработка изменения производителя
            if (Product.Manufacturer1 == null)
                Product.Manufacturer1 = new Manufacturer();

            Product.Manufacturer1.name = tbManufacturer.Text;
            Product.cost = decimal.Parse(tbCost.Text);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
