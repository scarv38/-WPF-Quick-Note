using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuickNote
{
	/// <summary>
	/// A base view model that fires Property Changed events as needed
	/// </summary>
	public class BaseViewModel : INotifyPropertyChanged
	{
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
	}
}
