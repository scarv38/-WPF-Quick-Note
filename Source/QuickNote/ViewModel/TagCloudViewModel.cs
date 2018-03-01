using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuickNote
{
	class TagCloudViewModel : BaseViewModel
	{
		//List of tags
		ObservableCollection<TagCloudItem> tagList;
		public ObservableCollection<TagCloudItem> TagList
		{
			get => tagList; set
			{
				tagList = value;
				OnPropertyChanged("TagList");
			}
		}

		//Main Window's View Model
		MainViewModel parentVM;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="temp"></param>
		public TagCloudViewModel(object temp)
		{
			parentVM = temp as MainViewModel;
			if (parentVM == null)
			{
				tagList = new ObservableCollection<TagCloudItem>();
				return;
			}

			parentVM.UpdateTagCloud += RefeshTagList;
			RefeshTagList(); //Intitial run
		}

		/// <summary>
		/// Refresh TagCloud's tag list when there's a change in the main window's tag list
		/// </summary>
		void RefeshTagList()
		{
			ObservableCollection<Tag> sourceList = parentVM.TagList;
			TagList = new ObservableCollection<TagCloudItem>();

			if (sourceList != null)
			{
				if(sourceList.Count > 0)
				{
					for (int i = 0; i < sourceList.Count; i++)
					{
						if(sourceList[i].NoteList.Count > 0)
						{
							TagList.Add(new TagCloudItem
							{
								TagName = sourceList[i].TagName,
								UsedTime = sourceList[i].NoteList.Count
							});
						}
					}
					UpdateFontSize();
				}
			}
		}

		/// <summary>
		/// Update font size for tag list
		/// </summary>
		void UpdateFontSize()
		{
			int max = TagList[0].UsedTime;
			for (int i = 1; i < TagList.Count; i++)
			{
				if (TagList[i].UsedTime > max)
					max = TagList[i].UsedTime;
			}

			int fMax = 100, fMin = 5;
			foreach (TagCloudItem tag in TagList)
			{
				tag.FontSize = (float)((float)((float)tag.UsedTime / (float)max) * (fMax - fMin) + fMin);
			}
		}

	}
}
