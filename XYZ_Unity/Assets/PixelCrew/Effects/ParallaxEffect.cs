using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float _effectValue;
    [SerializeField] private Transform _followTarget;

    private float _startX;
    // Start is called before the first frame update
    void Start()
    {
        _startX = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var currentPosition = transform.position;
        var deltaX = _followTarget.position.x * _effectValue;
        transform.position = new Vector3(_startX + deltaX, currentPosition.y, currentPosition.z);
    }
}
