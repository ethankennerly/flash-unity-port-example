using NUnit.Framework;
using System.Collections.Generic;

namespace Finegamedesign.Utils
{
	public sealed class TestStorage
	{
		[Test]
		public void Delete()
		{
			Storage storage = new Storage();
			storage.name = "test_delete";
			storage.Delete();
			Dictionary<string, object> data = storage.Load();
			Assert.AreEqual(0, DataUtil.Length(data));
			storage.SetKeyValue("level", 2);
			storage.Save();
			data = storage.Load();
			Assert.AreEqual(2, data["level"]);
			data = storage.Load();
			Assert.AreEqual(2, data["level"]);
			storage.Delete();
			data = storage.Load();
			Assert.AreEqual(0, DataUtil.Length(data));
			storage.Save();
			Assert.AreEqual(0, DataUtil.Length(data));
			data = storage.Load();
			Assert.AreEqual(0, DataUtil.Length(data));
		}
	}
}
