using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class MoveWithPlatform : MonoBehaviour
    {
        private Transform _originalParent;
        private Transform _newParent;



        private void Start()
        {
            _originalParent = transform.parent;
        }




        private void SetParent()
        {
            transform.parent = _newParent;
        }

        private void ResetParent()
        {
            transform.parent = _originalParent;
        }



        private void OnCollisionEnter2D(Collision2D collider)
        {
            if (collider.gameObject.tag == "MovablePlatform")
            {
                _newParent = collider.transform;
                SetParent();
            }
        }

        private void OnCollisionExit2D(Collision2D collider)
        {
            if (collider.gameObject.tag == "MovablePlatform")
            {
                _newParent = null;
                ResetParent();
            }
        }


    }




        /*{
        private Rigidbody2D _playerRBody;
        private Rigidbody2D _platformRBody;
        private CalculateVelocityFromPosition _objVelocity;
        private bool _isOnPlatform;



        private void Awake()
        {
            _playerRBody = GetComponent<Rigidbody2D>();
            //_objVelocity = GetComponent<CalculateVelocityFromPosition>();
        }

        void OnCollisionEnter2D(Collision2D collider)
        {
            if (collider.gameObject.tag == "MovablePlatform")
            {
                _objVelocity = collider.gameObject.GetComponent<CalculateVelocityFromPosition>(); //в этой строчке записываем компонент объекта, на который запрыгнул
                _isOnPlatform = true;
            }
        }

        private void OnCollisionStay2D(Collision2D collider)
        {
            if (collider.gameObject.tag == "MovablePlatform")
            {
                _playerRBody.velocity += _objVelocity.objVelocity;
                Debug.Log("Движемся на платформе");
            }

        }
        void OnCollisionExit2D(Collision2D collider)
        {
            if (collider.gameObject.tag == "MovablePlatform")
            {
                _objVelocity = null;
                _isOnPlatform = false;
            }
        }

        /*void FixedUpdate()
                {
                    if(_isOnPlatform)
                    {
                        _playerRBody.velocity += _objVelocity.objVelocity;
                    }
            Debug.Log(_playerRBody.velocity);
        }*/
        //}
    }
    
