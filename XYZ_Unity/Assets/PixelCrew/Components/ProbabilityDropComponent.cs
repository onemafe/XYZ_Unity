﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
	public class ProbabilityDropComponent : MonoBehaviour
	{
		[SerializeField] private int _count;
		[SerializeField] private DropData[] _drop;
		[SerializeField] private DropEvent _onDropCalculated;


		public void CalculateDrop()
		{
			var itemsToDrop = new GameObject[_count];
			var itemCount = 0;
			var total = _drop.Sum(dropData => dropData.Probability);
			var drop = _drop.OrderBy(dropData => dropData.Probability);


			/* while (itemCount < _count)
			{
				var random = UnityEngine.Random.value * total;
				foreach(var dropData in sortedDrop)
				{
					if (dropData.Probability >= random) //попали в нужное место
					{
						itemsToDrop[itemCount] = dropData.Drop;
						itemCount++;
						break;
					}	
				}
			}*/

			_onDropCalculated?.Invoke(itemsToDrop);
		}


		[Serializable]
		public class DropData
		{
			public GameObject Drop;
			[Range(0f,100f)] public float Probability;
		}

		[Serializable]
		public class DropEvent : UnityEvent<GameObject[]>
		{

		}
	}
}

