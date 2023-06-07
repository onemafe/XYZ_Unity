using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class PlatformMovementHorizontal : MonoBehaviour
    {
        [SerializeField] float offsetLeft = 0, offsetRight = 0, speed = 0;
        [SerializeField] bool hasReachedRight = false, hasReachedLeft = false;

        Vector3 startPosition = Vector3.zero;

        private void Awake()
        {
            startPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if(!hasReachedRight)
            {
                if(transform.position.x < startPosition.x + offsetRight)
                {
                    Move(offsetRight);
                }
                else if(transform.position.x >= startPosition.x + offsetRight)
                {
                    hasReachedRight = true;
                    hasReachedLeft = false;
                }
            }
            else if(!hasReachedLeft)
            {
                if(transform.position.x > startPosition.x + offsetLeft)
                {
                    Move(offsetLeft);
                }
                else if(transform.position.x <= startPosition.x + offsetLeft)
                {
                    hasReachedRight = false;
                    hasReachedLeft = true;
                }
            }
        }

        private void Move (float offset)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPosition.x + offset, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            float width = GetComponent<SpriteRenderer>().size.x;
            float heigth = GetComponent<SpriteRenderer>().size.y;

            float offsetLeftX = startPosition.x + offsetLeft - width/2;
            float offsetRightX = startPosition.x + offsetRight + width / 2;
            float offsetTopY = startPosition.y + heigth/2;
            float offsetBottomY = startPosition.y - heigth / 2;

            Gizmos.DrawLine(new Vector3(offsetLeftX, offsetTopY, 0), new Vector3(offsetLeftX, offsetBottomY, 0));
            Gizmos.DrawLine(new Vector3(offsetRightX, offsetTopY, 0), new Vector3(offsetRightX, offsetBottomY, 0));

        }

    }
}
