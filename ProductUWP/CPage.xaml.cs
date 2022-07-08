using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ProductUWP.ViewModels;

namespace ProductUWP
{
    public sealed partial class CPage : Page
    {
        public CPage() 
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void CSearch_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Refresh();
        }

        private void Add_To_Cart_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.ATC();
            }
        }

        private void Checkout(object sender, RoutedEventArgs e) 
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.Checkout();
            }
        }

        private void CRemove_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.CRemove();
            }
        }

        private void CEdit_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null )
            {
                vm.CUpdate();
            }
        }

        private void CSortN_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).CSortbyName();
        }
        public void SortbyTP_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).SortTP_Click();
        }

        private void CSave_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).CSave();
        }

        private void CLoad_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).CLoad();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
