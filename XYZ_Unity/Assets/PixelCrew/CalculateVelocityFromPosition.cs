using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.Components
{

    public class CalculateVelocityFromPosition : MonoBehaviour
    {
        Vector2 prevPos;
        Vector2 newPos;
        public Vector2 objVelocity;
        void Start()
        {
            prevPos = transform.position;
            newPos = transform.position;
        }

        private void FixedUpdate()
        {
            newPos = transform.position;     // each frame track the new position
            objVelocity = (newPos - prevPos)/ Time.fixedDeltaTime;   //velocity = dist/time
            prevPos = newPos;   // update position for next frame calculation
        }
    }
}
