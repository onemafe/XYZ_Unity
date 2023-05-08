using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Components
{
    public class SwitchComponent : MonoBehaviour
    {
    [SerializeField] private Animator _animation;
    [SerializeField] private bool _state;
    [SerializeField] private string _animationKey;

    public void Switch()
    {
        _state = !_state;
        _animation.SetBool(_animationKey, _state);
    }

    [ContextMenu("Switch")]
    public void SwitchIt()
        {
            Switch();
        }

    }

}
