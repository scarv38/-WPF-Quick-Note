using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNote
{
	public class NoteViewModel : BaseViewModel
	{
		/// <summary>
		/// Note List
		/// </summary>
		private ObservableCollection<Note> noteList;

		/// <summary>
		/// Get Note List
		/// </summary>
		public ObservableCollection<Note> NoteList { get => noteList; set => noteList = value; }

		#region Contructor
		/// <summary>
		/// Default contructor
		/// </summary>
		public NoteViewModel()
		{

		}
		#endregion
	}
}
