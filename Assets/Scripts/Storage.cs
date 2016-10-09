using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Finegamedesign.Utils
{
	// Save data to a file and load data from a file.
	// Adapted from:
	// Eric Daily15 May 2014
	// http://gamedevelopment.tutsplus.com/tutorials/how-to-save-and-load-your-players-progress-in-unity--cms-20934
	public sealed class Storage 
	{
		public Dictionary<string, object> data;
		public string name = "user";
		private FileStream file;
		private BinaryFormatter formatter = new BinaryFormatter();
		
		private string FormatPath()
		{
			return Application.persistentDataPath + "/" + name + ".data";
		}

		public void SetKeyValue(string key, object val)
		{
			if (null == data)
			{
				data = new Dictionary<string, object>();
			}
			data[key] = val;
		}

		public void Save(Dictionary<string, object> hash = null)
		{
			if (null != hash)
			{
				data = hash;
			}
			string path = FormatPath();
			if (null != file)
			{
			}
			if (File.Exists(path))
			{
				file = File.Open(path, FileMode.Open);
			}
			else
			{
				file = File.Create(path);
			}
			formatter.Serialize(file, data);
			file.Close();
		}
		 
		public Dictionary<string, object> Load()
		{
			string path = FormatPath();
			if (null != file)
			{
			}
			if (File.Exists(path))
			{
				file = File.Open(path, FileMode.Open);
				data = (Dictionary<string, object>)formatter.Deserialize(file);
				file.Close();
			}
			return data;
		}

		public void Delete()
		{
			data = new Dictionary<string, object>();
			Save();
			file = null;
		}
	}
}
