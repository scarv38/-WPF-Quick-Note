using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickNote
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		NotifyIcon nIcon;
		WindowState lastState;
		MainViewModel viewModel;
		public static bool isShutDown = false;

		public MainWindow()
		{
			InitializeComponent();
			NotifyIconInit();
			//Hide();
			AddNoteWindow.IsOpen = false;
			viewModel = new MainViewModel(this);
			this.DataContext = viewModel;
		}

		#region Hook
		bool []pressedKey = new bool[2];

		void CheckKeyUp(object sender, KeyUpArgs e)
		{
			if(e.KeyUp == Key.LeftShift || e.KeyUp == Key.RightShift || e.KeyUp == Key.F6)
			{
				pressedKey[0] = pressedKey[1] = false;
			}
		}

		void OpenAddWindow(object sender, KeyPressedArgs e)
		{
			if(e.KeyPressed == Key.LeftShift || e.KeyPressed == Key.RightShift)
			{
				pressedKey[0] = true;
			}
			else if(e.KeyPressed == Key.F6)
			{
				if(pressedKey[0])
					pressedKey[1] = true;

			}
			
			if(pressedKey[0] && pressedKey[1])
			{
				if (!AddNoteWindow.IsOpen)
				{
					AddNoteWindow dialog = new AddNoteWindow();
					dialog.ShowDialog();
					dialog.WindowState = WindowState.Normal;
					dialog.Focus();
				}
				else
				{
					AddNoteWindow dialog = this.FindName("AddNoteWin") as AddNoteWindow;
					if (dialog != null)
						dialog.WindowState = WindowState.Normal;
				}
				pressedKey[0] = pressedKey[1] = false;
			}

		}
		#endregion


		#region Notify Icon Settings
		/// <summary>
		/// Create NotifyIcon
		/// </summary>
		private void NotifyIconInit()
		{
			nIcon = new NotifyIcon();
			lastState = WindowState.Normal;
			nIcon.Visible = true;
			this.nIcon.Icon = QuickNote.Properties.Resources.app;



			System.Windows.Forms.ContextMenu nIconMenu = new System.Windows.Forms.ContextMenu();
			nIcon.DoubleClick += new EventHandler(OpenNIcon);
			nIconMenu.MenuItems.Add("Open", new EventHandler(OpenNIcon));
			nIconMenu.MenuItems.Add("Add (Shift + F6)").Click += (s,e) => viewModel.AddNoteNI();
			nIconMenu.MenuItems.Add("View Statistics").Click += (s,e)=>viewModel.TagCloudNI();
			nIconMenu.MenuItems.Add("Close", new EventHandler(ShutDownApp));

			nIcon.ContextMenu = nIconMenu;
		}

		/// <summary>
		/// Catch Stage Change events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_StateChange(object sender, EventArgs e)
		{
			switch (this.WindowState)
			{
				case WindowState.Minimized:
					this.Hide();
					break;
				default:
					lastState = this.WindowState;
					break;
			}
		}
		
		void ShutDownApp(object sender, EventArgs e)
		{
			isShutDown = true;
			//nIcon.Dispose();
			System.Windows.Application.Current.Shutdown();
		}

		/// <summary>
		/// Catch Closing event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
			if(!isShutDown)
				nIcon.ShowBalloonTip(300, "My Quick Note", "My Quick Note has minimized to taskbar", System.Windows.Forms.ToolTipIcon.Info);	
		}

		/// <summary>
		/// Choose Open on NotifyIcon menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OpenNIcon(object sender, EventArgs e)
		{
			this.Show();
			this.WindowState = lastState;
		}
		#endregion

		private void ListBox1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta < 0)
				ScrollViewer1.LineDown();
			else
				ScrollViewer1.LineUp();
		}

		private void ListBox2_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			 if (e.Delta < 0)
				ScrollViewer2.LineDown();
            else
                ScrollViewer2.LineUp();
		}

	
	}
}
