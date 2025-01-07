using chatbotWPF;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace chatbotWPF
{
    public partial class MainWindow : Window
    {
        FirstSend test = new FirstSend();
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = test;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (test.Target == 0)
            {
                Error.Text = "   Välj Person!";
            }

            else if (nameB.Text == "Ditt Namn")
            {
                Error.Text = "Skriv ett namn!";
            }

            else if (!string.IsNullOrEmpty(nameB.Text))
            {
                Enter.Visibility = Visibility.Hidden;
                Main.Visibility = Visibility.Visible;
            }

            test.Name = nameB.Text.ToString();
            user.Text = test.Name+" "+test.Target;
        }

        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            chatHistory.Text += textToSend.Text;
            textToSend.Text = string.Empty;
            //Enter.Visibility = Visibility.Visible;
            //Main.Visibility = Visibility.Hidden;
        }

        private void ClearTextOnFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = string.Empty;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //string fileName = $"{DateTime.Now.ToString("yyyy-M-dd--HH-mm-ss")}.txt";
            //string textToAdd = chatHistory.Text;
            //FileStream fs = null;
            //try
            //{
            //    fs = new FileStream(fileName, FileMode.CreateNew);
            //    using (StreamWriter writer = new StreamWriter(fs))
            //    {
            //        writer.Write(textToAdd);
            //    }
            //}
            //finally
            //{
            //    if (fs != null)
            //        fs.Dispose();
            //}

            this.Close();
        }

        private void Ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button1.Fill = Brushes.White;
            Button2.Fill = Brushes.White;
            Button3.Fill = Brushes.White;

            var clickEllipse = sender as Ellipse;

            if (clickEllipse == Button1)
            {
                Button1.Fill = Brushes.Blue;
                test.Target = 1;

            }
            if (clickEllipse == Button2)
            {
                Button2.Fill = Brushes.Blue;
                test.Target = 2;

            }
            if (clickEllipse == Button3)
            {
                Button3.Fill = Brushes.Blue;
                test.Target = 3;
            }
        }
    }
}