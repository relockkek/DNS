using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DNS;
using Newtonsoft.Json;

namespace DNS
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<CartItem> CartItems { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Products = new ObservableCollection<Product>();
            LoadProducts();

            CartItems = new ObservableCollection<CartItem>();
        }

        private void LoadProducts()
        {
            string json = File.ReadAllText("products.json");
            var products = JsonConvert.DeserializeObject<List<Product>>(json);
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var product = (Product)((Button)e.Source).CommandParameter;
            var cartItem = CartItems.FirstOrDefault(ci => ci.Product == product);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                CartItems.Add(new CartItem { Product = product, Quantity = 1 });
            }
        }

        private void OpenCart_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow(CartItems);
            cartWindow.Show();
        }
    }
}