using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;
        [SerializeField] private bool _isReadyToInteract = true;

        public void Interact()
        {
            if (_isReadyToInteract)
            {
                _action?.Invoke();
            }
            else return;
        }

        public void SetReady(bool isReady)
        {
            _isReadyToInteract = isReady;
        }
    }
}


