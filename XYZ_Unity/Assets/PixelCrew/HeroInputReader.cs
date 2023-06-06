using PixelCrew.Creatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{

    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;


        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }


        /*public void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _hero.SaySomething();
            }
        }*/
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Attack();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Interact();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.StartThrowing();
            }

            if (context.canceled)
            {
                _hero.PerformThrowing();
            }
        }



    }
}