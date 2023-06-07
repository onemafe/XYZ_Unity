using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
  
        [SerializeField] private float _interactionRadius;
        [SerializeField] private float _slamDownVelocity;
        private float _defaultGravityScale;

        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayerMask _interactionLayer;


        [SerializeField] private LayerCheck _wallCheck;

        [SerializeField] public int _knivesNumber;
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;


        [Header("Super throw")] [SerializeField]
        private Cooldown _superThrowCooldown;
        [SerializeField] private int _superThrowParticles;
        [SerializeField] private float _superThrowDelay;



        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

        //private Collider2D[] _interactionResult = new Collider2D[1];

        private bool allowDoubleJump;
        private bool _isOnWall;
        private bool _superThrow;

        //private bool _isArmed;
        //public int scores;

        private GameSession _session;



        protected override void Awake()// �������� ���������� ���� ��
        {
            base.Awake();

            _defaultGravityScale = Rigidbody.gravityScale;
        }



        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateHeroWeapon();

            var health = GetComponent<HealhComponent>();
            if (_session.Data.Hp <= 0)
            {
                _session.Data.Hp = _session.Data.MaxHp; 
            }
                
            health.SetHealth(_session.Data.Hp);

            _knivesNumber = _session.Data.Knives;
            
        }



        protected override void Update()
        {
            base.Update();


            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.isTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 1;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }

            Animator.SetBool(IsOnWall, _isOnWall);
        }



        protected override float CalculateYVelocity()
        {

            var isJumpPressing = Direction.y > 0;

            if (_IsGrounded || _isOnWall)
            {
                allowDoubleJump = true;
            }
            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }




        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!_IsGrounded && allowDoubleJump && !_isOnWall)
            {
                _particles.Spawn("Jump");
                allowDoubleJump = false;
                return _jumpSpeed;
            }
            return base.CalculateJumpVelocity(yVelocity);
        }




        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;

            base.Attack();
        }


        public void OnDoThrow()
        {
            if (_superThrow)
            {

                var numThrows = Mathf.Min(_superThrowParticles, _knivesNumber - 1);
                StartCoroutine(DoSuperThrow(numThrows));
            }

            else
            {
                _particles.Spawn("Throw");
                _knivesNumber -= 1;
                _session.Data.Knives = _knivesNumber;
            }
            _superThrow = false;
        }

        private IEnumerator DoSuperThrow (int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                _particles.Spawn("Throw");
                _knivesNumber -= 1;
                _session.Data.Knives = _knivesNumber;
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }


        /*public void Throw()
        {
            if (_throwCooldown.IsReady)
            {
                
                if (_knivesNumber > 1)
                {
                    Animator.SetTrigger(ThrowKey);
                    _throwCooldown.Reset();
                    _knivesNumber -= 1;
                    _session.Data.Knives = _knivesNumber;
                }
            }
        }*/

        public void StartThrowing()
        {
            _superThrowCooldown.Reset();
        }

        public void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || _knivesNumber <= 1)
            {
                return;
            }
            if (_superThrowCooldown.IsReady)
                _superThrow = true;

            if (_throwCooldown.IsReady)
            {

                if (_knivesNumber > 1)
                {
                    Animator.SetTrigger(ThrowKey);
                    _throwCooldown.Reset();
                }
            }
        }

        public void ShowScores()
        {
            Debug.Log($"��� ����: {_session.Data.Coins}");
        }




        public override void TakeDamage()
        {
            base.TakeDamage();

            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }



        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coins, 5);
        _session.Data.Coins -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }



        public void Interact()
        {
            _interactionCheck.Check();
        }



        private void OnCollisionEnter2D(Collision2D other)
        {
            if (IsGrounded())
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }

                if (contact.relativeVelocity.y >= _damageVelocity)
                {
                    GetComponent<HealhComponent>().ApplyDamage(2);
                }
            }
        }



        /*public void SpawnJumpDust()
        {
            _footJumpParticles.Spawn();
        }

        public void SpawnSwordAttackParticle()
        {
            _swordAttackParticle.Spawn();
        }*/



        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();

        }

        public void DisarmHero()
        {
            _session.Data.IsArmed = false;
            UpdateHeroWeapon();
        }



        private void UpdateHeroWeapon()
        {
            if (_session.Data.IsArmed)
            {
                Animator.runtimeAnimatorController = _armed;
            }
            else
            {
                Animator.runtimeAnimatorController = _disarmed;
            }
        }



        public void OnHealthChanged() //������ ��������� �� HealthComponent ����� HEro � Session. �������� ��� � HealtComponent.
        {
            var health = GetComponent<HealhComponent>();
            _session.Data.Hp = health._health;
        }

    }
}


