using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace QuickNote
{
    class AddNoteViewModel : BaseViewModel
    {
		public delegate void AddNoteDelegate(string[] tags, Note note);
		public event AddNoteDelegate SubmitNote;

		#region Binding Components
		/// <summary>
		/// Note's title
		/// </summary>
		string titleBox;
		/// <summary>
		/// The content of the note
		/// </summary>
		string noteBox;
		/// <summary>
		/// The list of tags, seperated by a ','
		/// </summary>
		string tagBox;
		#endregion

		#region BC Get/Set
		/// <summary>
		/// Titlebox
		/// </summary>
		public string TitleBox { get=>titleBox;
			set
			{
				titleBox = value;
				OnPropertyChanged("TitleBox");
			}
		}

		/// <summary>
		/// Notebox, can type multiple lines
		/// </summary>
		public string NoteBox
		{
			get => noteBox;
			set
			{
				noteBox = value;
				OnPropertyChanged("NoteBox");
			}
		}

		/// <summary>
		/// TagBox
		/// </summary>
		public string TagBox
		{
			get => tagBox;
			set
			{
				tagBox = value;
				OnPropertyChanged("TagBox");
			}
		}
		#endregion

		#region Commands

		public RelayCommand AddNoteCommand { get; set; }

		/// <summary>
		/// Check the conditions and add a new note to main window
		/// </summary>
		/// <param name="parameter"></param>
		void AddNewNote(object parameter)
		{
			//Check if all 3 boxes are filled
			if (string.IsNullOrWhiteSpace(TitleBox) || string.IsNullOrWhiteSpace(NoteBox) || string.IsNullOrWhiteSpace(TagBox))
			{
				MessageBox.Show("Note and Tags should not be empty.", "ERROR");
				return;
			}

			DateTime dateModified = DateTime.Now; //Get created date
			var newNote = new Note() { Title=TitleBox , Msg = NoteBox, Date = dateModified.ToString() }; //Create a new note

			var listTags = DataManager.StringToList(TagBox); //Convert raw text in TagBox to an array of string
			if (listTags == null) //Error checking
			{
				MessageBox.Show("Tags invaild.","ERROR");
				return;
			}

			//Write new note to database file
			bool newID = DataManager.WriteToFile(newNote, dateModified, listTags as string[]);
			if (newID)
			{
				//newNote.ID = newID.ToString();
				if (SubmitNote != null)
					SubmitNote(listTags as string[], newNote); //Call a submit note command to the mainwindow

				MessageBox.Show("Successfully added a new note!", "Add Note", MessageBoxButton.OK, MessageBoxImage.Information);
			}

			//Reset 3 boxes
			NoteBox = ""; 
			TagBox = "";
			TitleBox = "";
	
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor
		/// </summary>
		public AddNoteViewModel()
		{
			TitleBox = "";
			NoteBox = "";
			TagBox = "";
			AddNoteCommand = new RelayCommand(AddNewNote);
		}
		#endregion

	}
}
