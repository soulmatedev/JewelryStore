using JewelryStore.Database;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;

namespace JewelryStore.View
{
    public partial class AdminView : Window
    {
        private readonly TradeEntities entities;

        public ObservableCollection<Product> Products { get; set; }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set { selectedProduct = value; }
        }

        public AdminView(TradeEntities entities, User user)
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

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Product newProduct = new Product();
            ProductDialog dialog = new ProductDialog(newProduct, isEditing: false, entities);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    if (newProduct.Manufacturer1 != null)
                    {
                        var existingManufacturer = entities.Manufacturers.FirstOrDefault(m => m.name == newProduct.Manufacturer1.name);
                        if (existingManufacturer != null)
                        {
                            newProduct.Manufacturer1 = existingManufacturer;
                        }
                        else
                        {
                            entities.Manufacturers.Add(newProduct.Manufacturer1);
                        }
                    }
                    entities.Products.Add(newProduct);
                    entities.SaveChanges();
                    Products.Add(newProduct);
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                ProductDialog dialog = new ProductDialog(SelectedProduct, isEditing: true, entities);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        if (SelectedProduct.Manufacturer1 != null)
                        {
                            var existingManufacturer = entities.Manufacturers.FirstOrDefault(m => m.name == SelectedProduct.Manufacturer1.name);
                            if (existingManufacturer != null)
                            {
                                SelectedProduct.Manufacturer1 = existingManufacturer;
                            }
                            else
                            {
                                entities.Manufacturers.Add(SelectedProduct.Manufacturer1);
                            }
                        }
                        entities.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите продукт для редактирования");
            }
        }



        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                entities.Products.Remove(SelectedProduct);
                entities.SaveChanges();
                Products.Remove(SelectedProduct);
            }
            else
            {
                MessageBox.Show("Выберите продукт для удаления");
            }
        }
    }
}
