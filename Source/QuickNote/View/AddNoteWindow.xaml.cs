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
    /// Interaction logic for AddNoteWindow.xaml
    /// </summary>
    public partial class AddNoteWindow : Window
    {
		public static bool IsOpen { get; set; }

		public AddNoteWindow()
        {
            InitializeComponent();
			IsOpen = true;
			this.DataContext = new AddNoteViewModel();
        }

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			IsOpen = false;
		}
	}
}
