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

		private void Button_Send_Click(object sender, RoutedEventArgs e)
		{
			// Add message to chat history
			chatHistory.Text += textToSend.Text;
			textToSend.Text = string.Empty;

			// Simulated API call
			_ = APIConnection.Login("test", "test");

			Send prompt = new Send
			{
				Target = test.Target,
				Name = test.Name,
				Message = chatHistory.Text
			};

			_ = APIConnection.SendPrompt(prompt);
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
	}
}
