using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Components
{
    public class MoveToPoint : MonoBehaviour
    {
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _speed;
        

        Vector3 _startPosition = Vector3.zero;
        Vector3 _endPosition = Vector3.zero;

        private void Awake()
        {
            _startPosition = transform.position;
            _endPosition = _endPoint.position;
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position,_endPosition, _speed * Time.deltaTime);
        }

    }
}