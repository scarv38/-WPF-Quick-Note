using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNote
{
	public class Tag : INotifyPropertyChanged
	{
		//Tag name
		private string tagName;
		/// <summary>
		/// Get tagName
		/// </summary>
		public string TagName
		{
			get
			{
				return tagName;
			}

			set
			{
				tagName = value;
				OnPropertyChanged("TagName");
			}
		}

		//List of note that contains tagName
		private ObservableCollection<Note> noteList;
		public ObservableCollection<Note> NoteList
		{
			get => noteList;
			set
			{
				noteList = value;
				OnPropertyChanged("NoteList");

			}
		}

		#region Constructors
		/// <summary>
		/// Default contructor
		/// </summary>
		public Tag()
		{
			tagName = "";
			noteList = new ObservableCollection<Note>();
		}
		#endregion

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
	}
}
