using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuickNote
{
	/// <summary>
	/// Interaction logic for TagCloudWindow.xaml
	/// </summary>
	public partial class TagCloudWindow : Window
	{
		public static bool IsOpen { get; set; }


		public TagCloudWindow(object vm)
		{
			InitializeComponent();
			this.DataContext = new TagCloudViewModel(vm);
			IsOpen = true;
			this.Closing += Window_Closing;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			IsOpen = false;
		}

		private void ColorSelected(object sender, MouseEventArgs e)
		{
			Label a = sender as Label;
			a.Foreground = new SolidColorBrush(Colors.Red);
		}

		private void ColorLeave(object sender, MouseEventArgs e)
		{
			Label a = sender as Label;
			a.Foreground = new SolidColorBrush(Colors.Black);
		}
	}
}
