﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalLevitationComponent : MonoBehaviour
{

    [SerializeField] private float _frequency = 1f;
    [SerializeField] private float _amplitude = 1f;
    [SerializeField] private bool _randomize;


    private float _originalY;
    private Rigidbody2D _rigidbody;
    private float _seed;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _originalY = _rigidbody.position.y;
        if (_randomize)
        {
            _seed = Random.value * Mathf.PI * 2;
        }
    }

    void Update()
    {
        var pos = _rigidbody.position;
        pos.y = _originalY + _amplitude * Mathf.Sin(_seed + Time.time * _frequency);
        _rigidbody.MovePosition(pos);
    }
}
