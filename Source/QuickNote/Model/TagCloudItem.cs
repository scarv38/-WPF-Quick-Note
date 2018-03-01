using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNote
{
	public class TagCloudItem : INotifyPropertyChanged
	{
		//Tag name
		private string tagName;
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

		//Used time
		public int UsedTime{get;set;}

		//Font size
		private float fontSize;
		public float FontSize
		{
			get => fontSize; set
			{
				fontSize = value;
				OnPropertyChanged("FontSize");
			}
		}
	
		/// <summary>
		/// Default contructor
		/// </summary>
		public TagCloudItem()
		{
			tagName = "";
			fontSize = 0;
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
	}
}
