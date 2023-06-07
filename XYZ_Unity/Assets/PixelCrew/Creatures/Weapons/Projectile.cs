﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixelcrew.Creatures.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private bool _invertX;

        private Rigidbody2D _rigidbody;
        private int _direction;

        private void Start()
        {
            var mod = _invertX ? -1 : 1;
            _direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
            _rigidbody = GetComponent<Rigidbody2D>();
            var force = new Vector2(_direction * _speed, 0);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);

        }

        /*private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.x +=_direction * _speed;
            _rigidbody.MovePosition(position);
        }*/

    }
}


