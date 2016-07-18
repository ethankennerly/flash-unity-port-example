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
