using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DNS
{
    public partial class CartWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<CartItem> CartItems { get; set; }
        private decimal _totalSum;

        public decimal TotalSum
        {
            get => _totalSum;
            set
            {
                _totalSum = value;
                OnPropertyChanged();
            }
        }

        public CartWindow(ObservableCollection<CartItem> cartItems)
        {
            InitializeComponent();
            DataContext = this;

            CartItems = cartItems;
            CartItems.CollectionChanged += CartItems_CollectionChanged;
            foreach (var item in CartItems)
            {
                item.PropertyChanged += CartItem_PropertyChanged;
            }

            UpdateTotalSum();
        }

        private void CartItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<CartItem>())
                {
                    item.PropertyChanged += CartItem_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<CartItem>())
                {
                    item.PropertyChanged -= CartItem_PropertyChanged;
                }
            }

            UpdateTotalSum();
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateTotalSum();
        }

        private void UpdateTotalSum()
        {
            TotalSum = CartItems.Sum(item => item.TotalPrice);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}