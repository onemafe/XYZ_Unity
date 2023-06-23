using PixelCrew.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class PinkStarAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missHeroCooldown = 0.5f;
        [SerializeField] private Vector2 _collider2DSize;
        [SerializeField] private float _rollSpeed = 2.5f;

        [SerializeField] private GameObject _attackRange;

        private float startSpeed;
        private Coroutine _current;
        private GameObject _target;

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");
        private static readonly int BeforeAttack = Animator.StringToHash("prepare-to-roll");
        private static readonly int IsRollKey = Animator.StringToHash("isRoll");

        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;
        private Patrol _patrol;
        private bool _isDead;
        private CapsuleCollider2D _collider2D;


        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
            _collider2D = GetComponent<CapsuleCollider2D>();
        }


        private void Start()
        {
            StartState(_patrol.DoPatrol());
            startSpeed = _creature._speed;
            
        }


        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;
            _target = go;

            StartState(AgroToHero());
        }

        private IEnumerator AgroToHero()
        {
            LookAtHero();
            _particles.Spawn("Exclamation"); // вот тут надо добавить анимацию перед роллом
            _animator.SetTrigger(BeforeAttack);
            yield return new WaitForSeconds(_alarmDelay);


            StartState(RollToHero());
        }

        private void LookAtHero() //задаём направление движения
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }


        private IEnumerator RollToHero()
        {
            while (_vision.isTouchingLayer)
            {
                SetDirectionToTarget();
                _animator.SetBool(IsRollKey, true);
                _creature._speed = _rollSpeed;
                _attackRange.SetActive(true);

                //анимация ролла
                //увеличение скорости
                //включаем объект дамага, или респауним его

                yield return null;
            }

            
            
                _animator.SetBool(IsRollKey, false);
                _creature._speed = startSpeed;
                _attackRange.SetActive(false);
                //сбрасываем значение скорости обратно
                //анимация обратно
                //уничтожаем объект дамага

                _creature.SetDirection(Vector2.zero);
                _particles.Spawn("MissHero");
                yield return new WaitForSeconds(_missHeroCooldown);
            

            

            StartState(_patrol.DoPatrol());
        }




        /*private IEnumerator Attack()
        {
            while (_canAttack.isTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(RollToHero());
        }*/




        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();

            _creature.SetDirection(direction);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if(_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
          

        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);
            _collider2D.size = _collider2DSize;
            _attackRange.SetActive(false);

            _creature.SetDirection(Vector2.zero);
            if (_current != null)
                StopCoroutine(_current);
        }
    }
}





