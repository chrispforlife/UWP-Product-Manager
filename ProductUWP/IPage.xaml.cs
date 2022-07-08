using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ProductUWP.ViewModels;
using Library.TaskManagement.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ProductUWP
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IPage : Page
    {
        public IPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void ISearch_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Refresh();
        }

        private async void Add_Product_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                await vm.Add(ProductType.Product);
            }
        }

        private void IRemove_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.IRemove();
            }
        }

        private void IEdit_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.IUpdate();
            }
        }

        private void IBOGO_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.IBOGO();
            }
        }

        private void ISave_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).ISave();
        }

        private void ILoad_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).ILoad();
        }


        private void ISortN_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).ISortbyName();
        }

        private void SortP_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).SortbyPrice();
        }

                private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
