using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace QuickNote
{
	public static class DataManager
    {
		//Database Directory
		public static string databasePath = "content.dtb";

		//Main window's viewModel
		public static MainViewModel mainVM;

		#region Main Window (CheckTagExist,ReadFromFile,UpdateToFile)

		/// <summary>
		/// Check if tags already exists
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="listOfTags"></param>
		/// <returns></returns>
		public static int CheckTagExist(string tag, ObservableCollection<Tag> listOfTags)
		{
			for (int i = 0; i < listOfTags.Count; i++)
			{
				if (tag == listOfTags[i].TagName)
					return i;
			}

			return -1;
		}

		/// <summary>
		/// Read Intitial Database from file
		/// </summary>
		public static void ReadFromFile()
		{
			if (mainVM == null)
				return;

			FileStream fs = null;
			XDocument reader;
			try
			{
				fs = File.Open("content.dtb", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
				if (fs.Length == 0) //There's no file or file is empty by default (not by user deleting all text)
				{
					reader = new XDocument();
					reader.Add(new XElement("Database"));
					reader.Element("Database").Add(new XElement("TagList"));
					reader.Save(fs);
					fs.Close();
					return;
				}
				else
					reader = XDocument.Load(fs);

				fs.Seek(0, SeekOrigin.Begin);
				var iTags = reader.Descendants("TagList").FirstOrDefault().Descendants("Tag");

				//Add intitial tags
				foreach(var iTag in iTags)
					mainVM.TagList.Add(new Tag(){TagName = iTag.Value});


				var items = from q in reader.Descendants("Note")
							select new
							{
								ID = q.Attribute("ID").Value,
								Title = q.Element("Title").Value,
								Date = q.Element("Date").Value,
								Msg = q.Element("Msg").Value,
								Tags = q.Elements("Tag").ToList()
							};

				fs.Close();

				foreach (var item in items)
				{
					Note newNote = new Note()
					{
						ID = item.ID,
						Title = item.Title,
						Date = item.Date,
						Msg = item.Msg
					};

					string[] newTags = new string[item.Tags.Count];

					for (int i = 0; i < item.Tags.Count; i++)
					{
						newTags[i] = item.Tags[i].Value;
					}

					mainVM.AddTag(newTags as string[], newNote);
				}
			}
			catch(XmlException xe)
			{
				MessageBox.Show(xe.ToString(),"ERROR");
				if (fs != null)
				{
					fs.Close();
					XDocument writer = new XDocument();
					writer.Add(new XElement("Database"));
					writer.Element("Database").Add(new XElement("TagList"));
					writer.Save(databasePath);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(),"ERROR");
			}
			finally
			{
				if(fs != null)
					fs.Close();
			}

		}

		/// <summary>
		/// Update selected note to file
		/// </summary>
		/// <param name="note"></param>
		public static bool UpdateToFile(Note note)
		{
			try
			{
				XDocument writer = XDocument.Load(databasePath);

				var updatedNote = writer.Descendants("Note").Where(
					a => a.Attribute("ID").Value.Equals(note.ID)).FirstOrDefault();

				updatedNote.Element("Title").Value = note.Title;
				updatedNote.Element("Date").Value = note.Date;
				updatedNote.Element("Msg").Value = note.Msg;

				writer.Save(databasePath);
				return true;
			}
			catch(XmlException xe)
			{
				MessageBox.Show(xe.ToString(),"ERROR");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(),"ERROR");
			}

			return false;
		}

		#endregion

		#region Add Note Window (WriteToFile,StringToList)

		/// <summary>
		/// Write new note to file
		/// </summary>
		/// <param name="note"></param>
		/// <param name="date"></param>
		/// <param name="tags"></param>
		public static bool WriteToFile(Note note, DateTime date, string[] tags)
		{
			//string name = date.ToString("yyyyMMddHHmmss");
			FileStream fs = null;
			try
			{
				fs = File.Open(databasePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
				fs.Lock(0, fs.Length);
				XDocument writer;

				if (fs.Length == 0)
				{
					writer = new XDocument();
					writer.Add(new XElement("Database"));
					writer.Element("Database").Add(new XElement("TagList"));
					writer.Save(fs);
				}
				else
					writer = XDocument.Load(fs);

				fs.Seek(0, SeekOrigin.Begin);
				XElement newNote = new XElement("Note",
					new XElement("Title", note.Title),
					new XElement("Date", note.Date),
					new XElement("Msg", note.Msg));

				string newID = date.ToFileTime().ToString();

				//newID = Convert.ToInt64(lastNote.Attribute("ID").Value) + 1;

				newNote.SetAttributeValue("ID", newID);
				note.ID = newID;

				for (int i = 0; i < tags.Length; i++)
				{
					newNote.Add(new XElement("Tag", tags[i]));
				}

				writer.Element("Database").Add(newNote);

				writer.Save(fs);
				fs.Close();

				return true;
			}
			catch (XmlException e)
			{
				MessageBox.Show(e.ToString(),"ERROR");
				if (fs != null)
				{
					fs.Close();
					XDocument writer = new XDocument();
					writer.Add(new XElement("Database"));
					writer.Element("Database").Add(new XElement("TagList"));
					writer.Save(databasePath);
					MessageBox.Show("The database is reset due to fatal error.", "ERROR");
					mainVM.TagList.Clear();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(), "ERROR");
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}

			return false;
		}

		/// <summary>
		/// Convert a string of tags (seperated by ',') to a string array
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static object StringToList(string str)
		{
			str = str.ToLower();
			if (string.IsNullOrWhiteSpace(str))
				return null;

			str = string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
			var list = str.Split(',');
			list = list.Where(w => w != "").ToArray();
			if (list.Length < 1)
				return null;

			list = list.Distinct().ToArray();
			return list;
		}

		#endregion

	}
}
