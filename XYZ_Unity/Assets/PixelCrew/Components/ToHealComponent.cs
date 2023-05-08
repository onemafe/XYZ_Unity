using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{

    public class ToHealComponent : MonoBehaviour
    {
        [SerializeField] private int _healthPoints;

        public void AddHealthPoints(GameObject target)
        {
            var healthComponent = target.GetComponent<HealhComponent>();
            if (healthComponent != null)
            {
                healthComponent.AddHealthPoints(_healthPoints);
            }

        }
    }
}