using chatbotWPF;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace chatbotWPF
{
	public partial class MainWindow : Window
	{
		FirstSend test = new FirstSend();
		static string sessionId = null;

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
		}

        private async void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textToSend.Text)) 
            {
                MessageBox.Show("Write something!");
            }

            else
            {
                int count = 0;
                thinking(test.Target, count);
                count = 1;

                textToSend.IsEnabled = false;
                sendButton.IsEnabled = false;
                var prompt = new Send() { Name = test.Name, Message = textToSend.Text, SessionId = sessionId }; //Fixa denna

                chatHistory.Text += "You: " + textToSend.Text + "\n";
                var test2 = await APIConnection.SendPrompt(prompt, test.Target.ToString());
                sessionId = test2.SessionId;

                thinking(test.Target, count);

                chatHistory.Text += "Bot: " + test2.Response + "\n\n";
                chatHistory.ScrollToEnd();

                // kräver att ttsapi är igång
                //Task.Run(async () => { await APIConnection.GenerateTTS(test2.Response, test.Target.ToString()); });

                textToSend.Text = string.Empty;
                textToSend.IsEnabled = true;
                sendButton.IsEnabled = true;
            }
        }

		private void ClearTextOnFocus(object sender, RoutedEventArgs e)
		{
			((TextBox)sender).Text = string.Empty;
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			// Save to file or close application
			this.Close();
		}

		private void Ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            // Reset all ellipse fills to white and borders to transparent
			Border1.BorderBrush = Brushes.Transparent;
			Border2.BorderBrush = Brushes.Transparent;
			Border3.BorderBrush = Brushes.Transparent;
            var imageBrush = new ImageBrush();

            // Handle selection for the clicked ellipse
            if (sender == Button1)
			{
				Border1.BorderBrush = Brushes.Green;
                imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/38dbabe5-5ac4-4749-8adb-4b50e559be77.jpg"));
                test.Target = 1;
			}
			else if (sender == Button2)
			{
				Border2.BorderBrush = Brushes.Green;
                imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/download.jpg"));
                test.Target = 2;
			}
			else if (sender == Button3)
			{
				Border3.BorderBrush = Brushes.Green;
                imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/flat,750x,075,f-pad,750x1000,f8f8f8.jpg"));
                test.Target = 3;
			}
            buddy.Fill = imageBrush;

        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Fel hantering | Fel lösen / namn eller server problem, just nu bara Ja eller Nej
            var loginAttempt = await APIConnection.Login(loginName.Text, loginPass.Text);

            if (loginAttempt == true)
            {
                MessageBox.Show("True");
                Login.Visibility = Visibility.Collapsed;
                Enter.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("False");
            }
        }
		private void thinking(int person, int count)
		{
            var imageBrush = new ImageBrush();

			if(count == 0)      //Normal
			{
                switch (person)
                {
                    case 1:
                        imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/GettyImages-903030184-1024x683.png"));
                        buddy.Fill = imageBrush;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;

                }
            }
            
			else if (count == 1)    //Thinking 
			{
                switch (person)
                {
                    case 1:
                        imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/38dbabe5-5ac4-4749-8adb-4b50e559be77.jpg"));
                        buddy.Fill = imageBrush;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;

                }
            }
		}
    }
}
