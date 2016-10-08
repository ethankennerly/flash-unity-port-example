using UnityEngine;

namespace Finegamedesign.Utils
{
	public sealed class HintView : MonoBehaviour
	{
		public GameObject store;
		public GameObject exitButton;
		public GameObject[] productButtons;
		public GameObject[] storeButtons;
		public HintController controller = new HintController();
	}
}
