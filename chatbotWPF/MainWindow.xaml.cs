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
			user.Text = test.Name + " " + test.Target;
		}

        private async void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            var test = new Send() { Name = "Test", Message = textToSend.Text }; //Fixa denna

            chatHistory.Text += "You: " + textToSend.Text + "\n";
            var test2 = await APIConnection.SendPrompt(test, "1");

            chatHistory.Text += "Bot: " + test2.Response + "\n";
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
			// Save to file or close application
			this.Close();
		}

		private void Ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            // Reset all ellipse fills to white and borders to transparent
			Border1.BorderBrush = Brushes.Transparent;
			Border2.BorderBrush = Brushes.Transparent;
			Border3.BorderBrush = Brushes.Transparent;

			// Handle selection for the clicked ellipse
			if (sender == Button1)
			{
				Border1.BorderBrush = Brushes.Green;
				test.Target = 1;
			}
			else if (sender == Button2)
			{
				Border2.BorderBrush = Brushes.Green;
				test.Target = 2;
			}
			else if (sender == Button3)
			{
				Border3.BorderBrush = Brushes.Green;
				test.Target = 3;
			}
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

            //Send prompt = new Send();
            //prompt.Target = test.Target;
            //prompt.Name = test.Name;
            //prompt.Message = chatHistory.Text;
            //_ = APIConnection.SendPrompt(prompt);

            //Checka Inloggning
        }
    }
}
