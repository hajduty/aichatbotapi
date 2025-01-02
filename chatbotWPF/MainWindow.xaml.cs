using System.Windows;
using System.Windows.Controls;

namespace chatbotWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Enter.Visibility = Visibility.Hidden;
            Main.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Enter.Visibility = Visibility.Visible;
            Main.Visibility = Visibility.Hidden;
        }

        private void ClearTextOnFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = string.Empty;
        }
    }
}