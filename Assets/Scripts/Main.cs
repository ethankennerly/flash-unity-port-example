using UnityEngine;

namespace Finegamedesign.Anagram
{
	public sealed class Main : MonoBehaviour 
	{
		private AnagramController controller = new AnagramController();

		public void Start()
		{
			controller.view.Setup(this.gameObject);
			controller.Setup();
		}

		public void Update()
		{
			controller.Update(Time.deltaTime);
		}
	}
}
