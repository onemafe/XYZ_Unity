using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using UnityEngine;

public class AddForceComponent : MonoBehaviour
{
    [SerializeField] private Vector2 _vectorStart;
    private Vector2 _vectorCurrent;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        _vectorCurrent = new Vector2(_vectorStart.x * _transform.lossyScale.x, _vectorStart.y);
    }

    public void ApplyForce(GameObject target)
    {
        var rigidBody = target.GetComponent<Rigidbody2D>();
        

        if (rigidBody != null)
        {
            rigidBody.AddForce(_vectorCurrent, ForceMode2D.Force);
        }
    }
}
