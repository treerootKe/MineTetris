using DesignPattern;
using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public abstract class AbstractSameMovement:MonoSingleton<AbstractSameMovement>
    {
        protected static readonly int Movement = Animator.StringToHash("Movement");
        protected static readonly int Hit = Animator.StringToHash("Hit");
        protected static readonly int Dead = Animator.StringToHash("Dead");
        
        protected float moveSpeed;
        protected float currentSpeed;
        
        protected int health;
        protected int directionX;
        protected int directionY;
        protected float stunDuration;
        
        protected bool isStunned;
        
        protected Animator animatorGameObject;
        protected Rigidbody2D rigidBodyGameObject;

        protected override void Awake()
        {
            base.Awake();
            animatorGameObject = gameObject.GetComponent<Animator>();
            rigidBodyGameObject = transform.GetComponent<Rigidbody2D>();
        }
        
        protected float VelocityX
        {
            get => rigidBodyGameObject.velocity.x;
            set => rigidBodyGameObject.velocity = new Vector2(value, VelocityY);
        }

        protected float VelocityY
        {
            get => rigidBodyGameObject.velocity.y;
            set => rigidBodyGameObject.velocity = new Vector2(VelocityX, value);
        }
        
        protected abstract void UpdateMoveX(float moveSpeed);

        protected abstract void UpdateMoveY(float moveSpeed,float jumpPower = 0);
        
        protected abstract void UpdateMovement();
        
        protected abstract void UpdateDirection();

        public abstract void BeHit(int damage, Vector2 attackerPosition);
    }
}