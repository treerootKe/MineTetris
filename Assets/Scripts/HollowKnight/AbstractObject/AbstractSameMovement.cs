using DesignPattern;
using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public abstract class AbstractSameMovement:MonoSingleton<AbstractSameMovement>
    {
        protected static readonly int SpeedX = Animator.StringToHash("SpeedX");
        protected static readonly int SpeedY = Animator.StringToHash("SpeedY");
        protected static readonly int Dead = Animator.StringToHash("Dead");
        
        protected float moveSpeed;
        protected float currentSpeed;
        
        protected int health;
        protected int directionX;
        protected int directionY;
        protected float stunDuration;
        
        protected bool isStunned;
        
        protected Animator animatorGameObject;
        protected Rigidbody2D rigidbodyGameObject;
        
        protected float VelocityX
        {
            get => rigidbodyGameObject.velocity.x;
            set => rigidbodyGameObject.velocity = new Vector2(value, VelocityY);
        }

        protected float VelocityY
        {
            get => rigidbodyGameObject.velocity.y;
            set => rigidbodyGameObject.velocity = new Vector2(VelocityX, value);
        }
        
        protected abstract void UpdateMoveX(float moveSpeed);

        protected abstract void UpdateMoveY(float moveSpeed);
        
        protected abstract void UpdateVelocity();
        
        protected abstract void UpdateDirection();

        public abstract void BeHit(int damage, Vector2 attackerPosition);
    }
}