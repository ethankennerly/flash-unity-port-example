using UnityEngine/*<Mathf>*/;

namespace com.finegamedesign.anagram
{
	public class Referee
	{
		public int level = 0;
		public int levelMax = 0;
		public float radius = 
				0.03125f;
				// 0.0625f;
				// 0.1f;

		public int up(float performance, float performanceMax)
		{
			float normal = (float)((performance / performanceMax) - 0.5f) * 2.0f;
			normal = Mathf.Max(-1.0f, Mathf.Min(1.0f, normal));
			float progress = normal * radius;
			float remaining = ((float)levelMax - level) / levelMax;
			progress *= remaining;
			int add = (int)(progress * levelMax);
			add = Mathf.Max(1, add);
			Debug.Log("Referee.up: progress " + progress + " normal " + normal + " performance " + performance);
			return add;
		}
	}
}
