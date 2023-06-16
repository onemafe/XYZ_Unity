using PixelCrew.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{

    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] protected float _damageJumpSpeed;
        [SerializeField] private int _damage;

        [Header("Checkers")]
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;



        [SerializeField] protected float _damageVelocity;




        /*[SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _footJumpParticles;
        [SerializeField] private SpawnComponent _slamDownParticle;
        [SerializeField] private SpawnComponent _swordAttackParticle;*/

        protected Rigidbody2D Rigidbody;
        protected Vector2 Direction;
        protected Animator Animator;
        protected bool _IsGrounded;
        protected PlaySoundComponent Sounds;

        private static readonly int isRunning = Animator.StringToHash("isRunning");
        private static readonly int isGroundKey = Animator.StringToHash("isGround");
        private static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");
        private static readonly int hit = Animator.StringToHash("hit");
        private static readonly int attackKey = Animator.StringToHash("attack");


        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundComponent>();
        }


        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            _IsGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);


            Animator.SetBool(isRunning, Direction.x != 0);
            Animator.SetFloat(verticalVelocity, Rigidbody.velocity.y);
            Animator.SetBool(isGroundKey, _IsGrounded);

            UpdateSpriteDirection(Direction);
        }

        protected bool IsGrounded()
        {
            return _groundCheck.isTouchingLayer;
        }
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;


            if (isJumpPressing)
            {
                var isFalling = Rigidbody.velocity.y <= 0.001f;
                if (!isFalling) return yVelocity;

                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }


            else if (Rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }
            return yVelocity;
        }


        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (_IsGrounded)
            {
                yVelocity += _jumpSpeed;
                DoJumpVfx();
            }
            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            _particles.Spawn("Jump");
            Sounds.Play("Jump");
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }


        public virtual void TakeDamage()
        {
            Animator.SetTrigger(hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpSpeed);
        }



        public virtual void Attack()
        {
            Animator.SetTrigger(attackKey);
            Sounds.Play("Melee");
        }


        private void OnAttackRange()
        {
            _attackRange.Check();
        }

        public void OnDeadStopAnimation()
        {
            Animator.enabled = false;
        }
    }
}

