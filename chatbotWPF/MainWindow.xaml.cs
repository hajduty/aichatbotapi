﻿using chatbotWPF;
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
            chatHistory.Text += "You: " + textToSend.Text + "\n";
            chatHistory.Text += "Bot: " + "Jag håller med!" + "\n";
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

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var hand = await APIConnection.Login("test", "test");       //Ändra så den checkar vad du skriver

            if (hand == true) 
            {
                MessageBox.Show("True");
                Login.Visibility = Visibility.Collapsed;
                Enter.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("False");
            }

            //Send prompt = new Send();
            //prompt.Target = test.Target;
            //prompt.Name = test.Name;
            //prompt.Message = chatHistory.Text;
            //_ = APIConnection.SendPrompt(prompt);

            //Checka Inloggning
        }
    }
}