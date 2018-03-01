using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace QuickNote
{
	public class MainViewModel : BaseViewModel
	{
		#region Binding components
		/// <summary>
		/// List of tags
		/// </summary>
		private ObservableCollection<Tag> tagList;

		/// <summary>
		/// List of notes
		/// </summary>
		private ObservableCollection<Note> noteList;

		/// <summary>
		/// Content of a note
		/// </summary>
		string msgBox; //Textbox's content
		string titleBox; //Note's Title
		Tag selectedTag; //Selected Tag
		Note selectedNote; //Selected Note
		bool textBoxOn; //Textbox's state
		#endregion

		#region BC Get/Set
		/// <summary>
		/// Get Tag List
		/// </summary>
		public ObservableCollection<Tag> TagList
		{
			get => tagList;
			set
			{
				tagList = value;
				OnPropertyChanged("TagList");
			}
		}

		/// <summary>
		/// Get Note List
		/// </summary>
		public ObservableCollection<Note> NoteList
		{
			get => noteList;
			set
			{
				noteList = value;
				OnPropertyChanged("NoteList");
			}
		}

		/// <summary>
		/// Change MsgBox to match the selected Note
		/// </summary>
		public string MsgBox
		{
			get => msgBox;
			set
			{
				msgBox = value;
				OnPropertyChanged("MsgBox");
			}
		}

		/// <summary>
		/// Change TitleBox to match the selected Note
		/// </summary>
		public string TitleBox
		{
			get => titleBox;
			set
			{
				titleBox = value;
				OnPropertyChanged("TitleBox");
			}
		}

		/// <summary>
		/// Show the note list of the selected Tag
		/// </summary>
		public Tag SelectedTag
		{
			get => selectedTag;
			set
			{
				selectedTag = value;
				if(selectedTag != null)
					NoteList = SelectedTag.NoteList; //Set new Notelist

				SelectedNote = null; //Unselect the previous note
				TextBoxOn = false; //Textbox is blank
				MsgBox = "";
				TitleBox = "";
				OnPropertyChanged("SelectedTag");
			}
		}

		/// <summary>
		/// Change the MsgBox's content with the content of the selected Note
		/// </summary>
		public Note SelectedNote
		{
			get => selectedNote;
			set
			{
				selectedNote = value;
				TextBoxOn = true; //Textbox is on, the user is now able to edit and save the current note
				if (selectedNote != null)
				{
					MsgBox = selectedNote.Msg; //Show the content of selected Note in Textbox
					TitleBox = selectedNote.Title;
				}
				OnPropertyChanged("SelectedNote");
			}
		}

		/// <summary>
		/// Get textbox's state. If a note is selected => return TRUE, otherwise return FALSE.
		/// </summary>
		public bool TextBoxOn
		{
			get => textBoxOn;
			set
			{
				textBoxOn = value;
				OnPropertyChanged("TextBoxOn");
			}
		}
		#endregion

		#region Commands

		public RelayCommand SaveNoteCommand { get; set; }
		public RelayCommand DeleteNoteCommand { get; set; }

		public RelayCommand OpenAddNoteCommand { get; set; }
		public RelayCommand ViewStatCommand { get; set; }
		public RelayCommand AboutCommand { get; set; }
		public RelayCommand ExitCommand { get; set; }

		/// <summary>
		/// Save content of a note
		/// </summary>
		/// <param name="parameter"></param>
		void SaveNote(object parameter)
		{
			var temp = SelectedNote as Note; //Get Selected Note
			if (temp == null)
				return;

			DateTime curTime = DateTime.Now;

			temp.Title = TitleBox;
			temp.Msg = MsgBox; //Update new note content
			temp.Date = curTime.ToString(); //Update last modified date
			SelectedNote = temp; //Set new value for the selected note
			if (DataManager.UpdateToFile(temp)) //Write new data to database file
				System.Windows.MessageBox.Show("Note saved successfully!", "Save Note", MessageBoxButton.OK, MessageBoxImage.Information);
			else
				System.Windows.MessageBox.Show("Cannot save note.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Information);

		}

		void DeleteNote(object parameter)
		{
			var temp = SelectedNote as Note; //Get Selected Note
			if (temp == null)
				return;

			if (System.Windows.MessageBox.Show("Do you want to delete this note?", "Delete note", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
				return;

			try
			{
				XDocument writer = XDocument.Load(DataManager.databasePath);
				XElement note = writer.Descendants("Note").Where
					(n => n.Attribute("ID").Value.Equals(temp.ID)).FirstOrDefault();

				note.Remove();
				writer.Save(DataManager.databasePath);
				SelectedNote = null;
				SelectedTag = null;
				MsgBox = TitleBox = "";

				foreach(Tag tag in TagList)
					tag.NoteList.Clear();

				TagList.Clear();
				DataManager.ReadFromFile();
				System.Windows.MessageBox.Show("Note deleted successfully!", "Delete Note", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception e)
			{
				System.Windows.MessageBox.Show(e.ToString(), "ERROR");
				System.Windows.MessageBox.Show("Cannot delete this note", "ERROR");
			}

		}

		void OpenAddNote(object parameter)
		{
			AddNoteNI();
		}

		void OpenStat(object parameter)
		{
			TagCloudNI();
		}

		void About(object parameter)
		{
			System.Windows.MessageBox.Show("Quick Note application for Windows\nMade by Vo Huu Thang\nStudent ID: 1512526\nHo Chi Minh City University of Science", "About",
				MessageBoxButton.OK, MessageBoxImage.Information);
		}

		void ExitApp(object parameter)
		{
			if(System.Windows.MessageBox.Show("Do you want to exit the application?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				MainWindow.isShutDown = true;
				System.Windows.Application.Current.Shutdown();
			}

		}
		#endregion

		#region Contructor
		/// <summary>
		/// Default contructor
		/// </summary>
		public MainViewModel(MainWindow mainview)
		{
			tagList = new ObservableCollection<Tag>(); //Create TagList
			SaveNoteCommand = new RelayCommand(SaveNote); //Create Command function for Save Button
			DeleteNoteCommand = new RelayCommand(DeleteNote); //Create Command function for Delete Button

			OpenAddNoteCommand = new RelayCommand(OpenAddNote); //Create Command function for Save Button
			ViewStatCommand = new RelayCommand(OpenStat); //Create Command function for Save Button
			AboutCommand = new RelayCommand(About); //Create Command function for Save Button
			ExitCommand = new RelayCommand(ExitApp); //Create Command function for Save Button

			TextBoxOn = false; //Textbox is blank
			DataManager.mainVM = this;
			DataManager.ReadFromFile(); //Import intital database

			_listener = new KeyBoardHook(); //Create a new Keyboard Hook
			_listener.OnKeyPressed += OpenAddWindow; //Create KeyPressed event
			_listener.OnKeyUp += CheckKeyUp; //Create KeyUp event
			_listener.HookKeyboard(); //Start listening to keyboard inputs
		}
		#endregion

		#region Linked functions

		public delegate void AddTagDelegate();
		public event AddTagDelegate UpdateTagCloud;

		/// <summary>
		/// Add new tags from AddNote window to Main window
		/// </summary>
		/// <param name="tags"></param>
		/// <param name="note"></param>
		public void AddTag(string[] tags,Note note)
		{
			try
			{
				XDocument writer = XDocument.Load(DataManager.databasePath);

				var updatedNote = writer.Descendants("TagList").FirstOrDefault();

				for (int i = 0; i < tags.Length; i++)
				{
					int pos = DataManager.CheckTagExist(tags[i], tagList);
					if (pos == -1) //if Tag doesn't exist in the current list
					{
						Tag temp = new Tag
						{
							TagName = tags[i]
						};

						temp.NoteList.Add(note);
						tagList.Add(temp);

						//Add new tag in file
						updatedNote.Add(new XElement("Tag", tags[i]));
						writer.Save(DataManager.databasePath);
					}
					else //Tag is already in list
					{
						tagList[pos].NoteList.Add(note);
					}

				}

				TagList = new ObservableCollection<Tag>(TagList.OrderBy(a => a.TagName)); //Sort taglist by alphabet

				if (UpdateTagCloud != null)
					UpdateTagCloud();

			}
			catch(XmlException e)
			{
				System.Windows.MessageBox.Show(e.ToString(), "ERROR");
				System.Windows.MessageBox.Show("Cannot add new tag", "ERROR");
			}

		}
		#endregion

		#region Hook
		private KeyBoardHook _listener;
		bool[] pressedKey = new bool[2];

		/// <summary>
		/// Check Keyboard Up
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void CheckKeyUp(object sender, KeyUpArgs e)
		{
			if (e.KeyUp == Key.LeftShift || e.KeyUp == Key.RightShift || e.KeyUp == Key.F6)
			{
				pressedKey[0] = pressedKey[1] = false;
			}
		}

		/// <summary>
		/// Check if the user presses the correct hot-keys. If it's true, open the AddNote Window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OpenAddWindow(object sender, KeyPressedArgs e)
		{
			if (e.KeyPressed == Key.LeftShift || e.KeyPressed == Key.RightShift)
			{
				pressedKey[0] = true;
			}
			else if (e.KeyPressed == Key.F6)
			{
				if (pressedKey[0]) //Check if Shift is pressed and hold before
					pressedKey[1] = true;
			}

			if (pressedKey[0] && pressedKey[1])
			{
				if (!AddNoteWindow.IsOpen) //if AddNote Window isn't already opened
				{
					AddNoteWindow dialog = new AddNoteWindow();
					AddNoteViewModel vm = dialog.DataContext as AddNoteViewModel;
					vm.SubmitNote += AddTag; //Forward the SubmitNote command to the Addtag function

					dialog.Show();
					dialog.Focus();
					dialog.WindowState = WindowState.Normal;
				}

				pressedKey[0] = pressedKey[1] = false; //return the key state to normal state
			}
		}
		#endregion

		#region NotifyIcon

		/// <summary>
		/// Choose Add on NotifyIcon menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void AddNoteNI()
		{
			if (!AddNoteWindow.IsOpen) 
			{
				AddNoteWindow dialog = new AddNoteWindow();
				AddNoteViewModel vm = dialog.DataContext as AddNoteViewModel;
				vm.SubmitNote += AddTag; //Forward the SubmitNote command to the Addtag function
				dialog.Show();
				dialog.Focus();
			}
		}

		/// <summary>
		/// Choose View Statistics on NotifyIcon menu (Open Tag Cloud)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void TagCloudNI()
		{
			if (!TagCloudWindow.IsOpen)
			{
				TagCloudWindow dialog = new TagCloudWindow(this);
				if (UpdateTagCloud != null)
					UpdateTagCloud(); //Call the UpdateTagCloud command to the new window
				dialog.Show();
				dialog.Focus();
			}
		}

		#endregion
	}
}
