using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public abstract class AbstractEnemy: MonoBehaviour,IObjectsMove
    {
        protected static readonly int Attack = Animator.StringToHash("Attack");
        protected static readonly int Dead = Animator.StringToHash("Dead");
        protected static readonly int Movement = Animator.StringToHash("Movement");
        protected static readonly int Hit = Animator.StringToHash("Hit");
        
        protected string Name;
        protected int Health;
        protected int Damage;
        protected float StunDuration;
        protected float MoveSpeed;
        protected float AttackingMoveSpeed;
        protected bool IsFly;
        protected bool IsStunned;
        
        protected Vector2 MovementDirection;
        
        protected Animator AnimatorEnemy;
        protected Rigidbody2D RigidbodyEnemy;
        protected float VelocityX
        {
            get => RigidbodyEnemy.velocity.x;
            set => RigidbodyEnemy.velocity = new Vector2(value, VelocityY);
        }

        protected float VelocityY
        {
            get => RigidbodyEnemy.velocity.y;
            set => RigidbodyEnemy.velocity = new Vector2(VelocityX, value);
        }

        public void UpdateMove(float speed)
        {
            if (MovementDirection.x == 0 && MovementDirection.y == 0)
            {
                return;
            }
            VelocityX = speed * MovementDirection.x;
            VelocityY = speed * MovementDirection.y;
        }

        public void UpdateDirection()
        {
            
        }

        public  void UpdateMovement()
        {
            
        }

        public void UpdateJump()
        {
            
        }

        public  void UpdateGravityScale()
        {
            
        }
        
        public abstract void AttackBehaviour(Transform transPlayer);
        
        public abstract void StopAttacking(bool isBeHit);
        
        public abstract void BeHit(int hitDamage, Vector2 posPlayer);
    }
}