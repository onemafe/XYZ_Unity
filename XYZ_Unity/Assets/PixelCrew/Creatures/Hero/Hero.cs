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
    public class Hero : Creature, ICanAddInInventory
    {
  
        [SerializeField] private float _interactionRadius;
        [SerializeField] private float _slamDownVelocity;
        private float _defaultGravityScale;

        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayerMask _interactionLayer;


        [SerializeField] private LayerCheck _wallCheck;

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


        private int CoinsCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count("Sword");

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
            if (_session.Data.Hp.Value <= 0)
            {
                _session.Data.Hp.Value = _session.Data.MaxHp; 
            }
                
            health.SetHealth(_session.Data.Hp.Value);

            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.Data.Inventory.OnChanged += AnotherHandler;

        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
            {
                UpdateHeroWeapon();
            }
        }

        private void AnotherHandler(string id, int value)
        {
            Debug.Log($"Inventory Changed: {id}: {value}");
        }

        private void OnDestroy() //чтобы не нагружалась память
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
            _session.Data.Inventory.OnChanged -= AnotherHandler;
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
                DoJumpVfx();
                allowDoubleJump = false;
                return _jumpSpeed;
            }
            return base.CalculateJumpVelocity(yVelocity);
        }




        public override void Attack() // SwordCount искать ниже у аниматоров атаки
        {
            if (SwordCount <= 0) return;

            base.Attack();
        }




        public void OnDoThrow()
        {
            if (_superThrow)
            {

                var numThrows = Mathf.Min(_superThrowParticles, SwordCount - 1);
                StartCoroutine(DoSuperThrow(numThrows));
            }

            else
            {
                _particles.Spawn("Throw");
                Sounds.Play("Range");
                RemoveFromInventory("Sword", 1);
            }
            _superThrow = false;
        }




        private IEnumerator DoSuperThrow (int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                _particles.Spawn("Throw");
                Sounds.Play("Range");
                RemoveFromInventory("Sword", 1);
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
            if (!_throwCooldown.IsReady || SwordCount <= 1)
            {
                return;
            }
            if (_superThrowCooldown.IsReady)
                _superThrow = true;

            if (_throwCooldown.IsReady)
            {

                if (SwordCount > 1)
                {
                    Animator.SetTrigger(ThrowKey);
                    _throwCooldown.Reset();
                }
            }
        }

        public void ShowScores()
        {
            Debug.Log($"��� ����: {CoinsCount}");
        }




        public override void TakeDamage()
        {
            base.TakeDamage();

            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
        }


        


        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinsCount, 5);

            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);


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



        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public void RemoveFromInventory(string id, int value)
        {
            _session.Data.Inventory.Remove(id, value);
        }




        private void UpdateHeroWeapon()
        {
            if (SwordCount > 0)
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
            _session.Data.Hp.Value = health._health;
        }

    }
}


