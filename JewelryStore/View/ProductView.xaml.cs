using JewelryStore.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace JewelryStore.View
{
    public partial class ProductView : Window
    {
        private readonly TradeEntities entities;
        public ObservableCollection<Product> Products { get; set; }
        public ProductView(TradeEntities entities, User user)
        {
            InitializeComponent();

            this.entities = entities;

            Products = new ObservableCollection<Product>(entities.Products);

            DataContext = this;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                Owner.Show();
            }
        }
    }
}
