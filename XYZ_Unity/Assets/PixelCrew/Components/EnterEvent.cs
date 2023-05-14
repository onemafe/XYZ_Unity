using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace PixelCrew.Components
{
    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {
    }
}

