using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace Finegamedesign.Utils
{
	[TestFixture]
	public sealed class TestAnagramLevelSelectView
	{
		[Test]
		public void Select107of109Unlocked()
		{
			TestLevelSelectView.Validate();
			LevelSelectController controller = new LevelSelectController();
			TestLevelSelectModel.Configure(controller.model);
			controller.Setup();
			Assert.AreEqual(false, controller.view == null);
			Assert.AreEqual(3, DataUtil.Length(controller.view.buttons));
			Assert.AreEqual("chapterSelect", controller.model.menuName);
			Assert.AreEqual(8, DataUtil.Length(controller.view.buttons[0]));
			controller.buttons.view.Down(controller.view.buttons[0][0]);
			controller.Update();
			Assert.AreEqual("levelSelect", controller.model.menuName);
			Assert.AreEqual(20, DataUtil.Length(controller.view.buttons[1]));
			controller.buttons.view.Down(controller.view.buttons[1][5]);
			controller.Update();
			Assert.AreEqual("wordSelect", controller.model.menuName);
			Assert.AreEqual(20, DataUtil.Length(controller.view.buttons[2]));
			controller.buttons.view.Down(controller.view.buttons[2][7]);
			controller.Update();
			Assert.AreEqual("play", controller.model.menuName);
			Assert.AreEqual(107, controller.model.levelSelected);
		}
	}
}
