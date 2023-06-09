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
        [SerializeField] private SpawnComponent _throwSpawner;

        private const string SwordId = "Sword";



        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

        //private Collider2D[] _interactionResult = new Collider2D[1];

        private bool allowDoubleJump;
        private bool _isOnWall;
        private bool _superThrow;


        private int CoinsCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;

        private bool CanThrow //какой-то флаг
        {
            get
            {
                if(SelectedItemId == SwordId)
                {
                    return SwordCount > 1;
                }

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

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
            if (id == SwordId)
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
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount; 

                var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
                StartCoroutine(DoSuperThrow(numThrows));
            }

            else
            {
                ThrowAndRemoveFromInventory();
            }
            _superThrow = false;
        }




        private IEnumerator DoSuperThrow (int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            //_particles.Spawn("Throw");

            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.ThrowableItems.Get(throwableId);

            _throwSpawner.SetPrefab(throwableDef.Projectile);
            _throwSpawner.Spawn();
            Sounds.Play("Range");
            RemoveFromInventory(throwableId, 1);
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



        public void UseInventory()
        {
            var isThrowable = _session.QuickInventory.SelectedDef.HasTag(ItemTag.Throwable);
            if (IsSelectedItem(ItemTag.Throwable))
                PerformThrowing();

            else if (IsSelectedItem(ItemTag.Potion))
                UsePotion();

        }



        private readonly Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed;



        protected override float CalculateSpeed()
        {
            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;
            return base.CalculateSpeed() + _additionalSpeed;
        }



        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch(potion.Effect)
            {
                case Effect.AddHp:
                    _session.Data.Hp.Value += (int) potion.Value;
                    break;

                case Effect.SpeedUp:
                    _speedUpCooldown.Value = _speedUpCooldown.TimeLasts + potion.Time;
                    _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                    _speedUpCooldown.Reset();
                    break;
            }

            _session.Data.Inventory.Remove(potion.Id, 1);
        }




        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }

        private void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || !CanThrow)
            {
                return;
            }
            if (_superThrowCooldown.IsReady)
                _superThrow = true;

            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
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

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }





    }
}


