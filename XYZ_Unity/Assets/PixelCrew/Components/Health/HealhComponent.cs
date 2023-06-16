using PixelCrew.Creatures;
using PixelCrew.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealhComponent : MonoBehaviour
    {
        [SerializeField] public int _health;
        //[SerializeField] private Hero _hero;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        /*[SerializeField] private HealthChangeEvent _onChange;*/

        private GameSession _session;
        private Hero _hero;

        private int BottleHealthCount => _session.Data.Inventory.Count("BottleHealth");

        [SerializeField] private int _bottleHeal;


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _hero = GetComponent<Hero>();
        }

        [ContextMenu("Kill")]
        public void Kill()
        {
            ApplyDamage(_health);
        }



        public void ApplyDamage(int _damageValue)
        {
            if (_health <= 0) return;

            _health -= _damageValue;
            _onDamage?.Invoke();
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
            Debug.Log("HP:" + _health);
            /*if (_hero != null)
            {
                _hero.OnHealthChanged();
            }*/
        }

        public void HealByBottle()
        {
            if (BottleHealthCount > 0)
            {
                _session.Data.Inventory.Remove("BottleHealth", 1);
                AddHealthPoints(_bottleHeal);
            }
        }

        public void AddHealthPoints(int _healthValue)
        {

            _health += _healthValue;
            _onHeal?.Invoke();
            Debug.Log(_health);
            //_hero.OnHealthChanged();
        }



        public void SetHealth(int health) //В классе Hero обращаемся к этому методу чтобы установить жизни. Там они берутся из Gamesession.
        {
            _health = health;
        }


        //что это?
        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
        }

        /*[Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }*/

    }
}

