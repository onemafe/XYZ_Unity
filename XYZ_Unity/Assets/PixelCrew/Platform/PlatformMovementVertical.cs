using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class PlatformMovementVertical
        : MonoBehaviour
    {
        [SerializeField] float offsetBottom = 0, offsetTop = 0, speed = 0;
        [SerializeField] bool hasReachedTop = false, hasReachedBottom = false;

        Vector3 startPosition = Vector3.zero;

        private void Awake()
        {
            startPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if(!hasReachedTop)
            {
                if(transform.position.y < startPosition.y + offsetTop)
                {
                    Move(offsetTop);
                }
                else if(transform.position.y >= startPosition.y + offsetTop)
                {
                    hasReachedTop = true;
                    hasReachedBottom = false;
                }
            }
            else if(!hasReachedBottom)
            {
                if(transform.position.y > startPosition.y + offsetBottom)
                {
                    Move(offsetBottom);
                }
                else if(transform.position.y <= startPosition.y + offsetBottom)
                {
                    hasReachedTop = false;
                    hasReachedBottom = true;
                }
            }
        }

        private void Move (float offset)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPosition.x, transform.position.y + offset, transform.position.z), speed * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            float width = GetComponent<SpriteRenderer>().size.x;
            float heigth = GetComponent<SpriteRenderer>().size.y;

            float offsetLeftX = startPosition.x - width/2;
            float offsetRightX = startPosition.x + width / 2;
            float offsetTopY = startPosition.y + + offsetTop + heigth/2;
            float offsetBottomY = startPosition.y + offsetBottom - heigth / 2;

            Gizmos.DrawLine(new Vector3(offsetLeftX, offsetTopY, 0), new Vector3(offsetRightX, offsetTopY, 0));
            Gizmos.DrawLine(new Vector3(offsetLeftX, offsetBottomY, 0), new Vector3(offsetRightX, offsetBottomY, 0));

        }

    }
}
