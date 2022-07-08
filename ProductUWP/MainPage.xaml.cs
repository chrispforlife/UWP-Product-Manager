using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ProductUWP.ViewModels;

namespace ProductUWP
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Employee_Click(object sender, RoutedEventArgs e) 
        {
            Frame.Navigate(typeof(IPage));
        }

        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CPage));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }


    }
}
