using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNote
{
	public class Note : INotifyPropertyChanged
	{
		//Note ID
		public string ID { get; set; }

		//Note content
		string msg;
		public string Msg
		{
			get=> msg;
			set
			{
				msg = value;
				OnPropertyChanged("Msg");
				OnPropertyChanged("ShowMsg");
			}
		}

		//Note title
		string title;
		public string Title { get => title;
			set
			{
				title = value;
				OnPropertyChanged("Title");
			}
		}

		//Date created
		string date;
		public string Date
		{
			get => date; set
			{
				date = value;
				OnPropertyChanged("Date");
			}
		}

		/// <summary>
		/// Get Shorted Msg
		/// </summary>
		/// <returns></returns>
		public string ShowMsg
		{
			get
			{
				if (Msg.Length <= 50)
					return Msg;

				return Msg.Substring(0, 50) + "...";
			}
			set
			{
				OnPropertyChanged("ShowMsg");
			}

		}

		#region Contructor
		/// <summary>
		/// Default contructor
		/// </summary>
		public Note()
		{
			msg = "";
			title = "";
			date = "";
		}
		#endregion

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
