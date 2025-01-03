using UnityEngine;

namespace HollowKnight.AbstractClass
{
    public abstract class AbstractEnemy: MonoBehaviour
    {
        protected static readonly int Attack = Animator.StringToHash("Attack");
        protected static readonly int Dead = Animator.StringToHash("Dead");
        
        protected string Name;
        protected int Health;
        protected int Damage;
        protected float Speed;
        protected float AttackSpeed;
        
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
        
        protected  Collider2D Collider2DAttackRange;
        
        public abstract void AttackBehaviour(Transform transPlayer);
        public abstract void BeHit(int hitDamage, Vector2 posPlayer);
    }
}