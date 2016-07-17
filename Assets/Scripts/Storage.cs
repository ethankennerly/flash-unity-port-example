using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// Save data to a file and load data from a file.
// Adapted from:
// Eric Daily15 May 2014
// http://gamedevelopment.tutsplus.com/tutorials/how-to-save-and-load-your-players-progress-in-unity--cms-20934
public class Storage 
{
	public Dictionary<string, object> data;
	public string name = "user";
	
	private string FormatPath()
	{
		return Application.persistentDataPath + "/" + name + ".data";
	}

	public void SetKeyValue(string key, object val)
	{
		if (null == data) {
			data = new Dictionary<string, object>();
		}
		data[key] = val;
	}

	public void Save(Dictionary<string, object> hash)
	{
		this.data = hash;
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream file = File.Create(FormatPath());
		formatter.Serialize(file, data);
		file.Close();
	}
	 
	public Dictionary<string, object> Load()
	{
		if (File.Exists(FormatPath())) {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(FormatPath(), FileMode.Open);
			data = (Dictionary<string, object>)formatter.Deserialize(file);
			file.Close();
		}
		return data;
	}
}
