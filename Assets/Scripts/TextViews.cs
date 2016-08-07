using UnityEngine/*<GameObject>*/;
using UnityEngine.UI/*<Text>*/;
using System.Collections.Generic/*<List>*/;

namespace Finegamedesign.Utils
{
	public sealed class TextViews
	{
		public static void SetChildren(List<GameObject> views, List<string> texts, string textNodeName = "text") 
		{
			for (int i = 0; i < DataUtil.Length(views); i++)
			{
				var view = views[i];
				if (null != view)
				{
					bool visible = i < texts.Count;
					SceneNodeView.SetVisible(view, visible);
					if (visible) {
						var textView = SceneNodeView.GetChild(view, textNodeName);
						TextView.SetText(textView, texts[i]);
					}
				}
			}
		}
	}
}
