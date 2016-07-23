using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace /*<com>*/Finegamedesign.Utils
{
	// Swap positions of several objects.
	// Tween the swapping.
	public sealed class TweenSwap
	{
		public float duration = 0.5f;
		private List<Vector3> stations;
		private List<GameObject> movers;

		public void Setup(List<GameObject> originals)
		{
			movers = new List<GameObject>();
			stations = new List<Vector3>();
			for (int index = 0; index < originals.Count; index++)
			{
				GameObject original = originals[index];
				movers.Add(original);
				Vector3 station = new Vector3();
				station.x = original.transform.localPosition.x;
				station.y = original.transform.localPosition.y;
				station.z = original.transform.localPosition.z;
				stations.Add(station);
			}
		}

		// Kill previous tweens.
		// Instantly return to original positions.
		// Test case:  2016-07-17 Word Garden.
		// Enter shorter word.  Swap positions.
		// Unknown step.  Enter full word.
		// Next word.  Expect no letters overlapping.
		// One of the five letters was hidden behind another letter.
		public void Reset()
		{
			DOTween.CompleteAll();
			for (int index = 0; index < movers.Count; index++)
			{
				SceneNodeView.SetLocal(movers[index], 
					stations[index]);
			}
		}

		public void Move(List<int> stationIndexes)
		{
			for (int s = 0; s < stationIndexes.Count; s++)
			{
				int stationIndex = stationIndexes[s];
				Vector3 station = stations[stationIndex];
				movers[s].transform.DOLocalMove(station, duration);
			}

		}
	}
}
