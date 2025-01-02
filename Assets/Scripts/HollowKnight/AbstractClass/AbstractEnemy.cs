using UnityEngine;

namespace HollowKnight.AbstractClass
{
    public abstract class AbstractEnemy: MonoBehaviour
    {
        protected static readonly int Dead = Animator.StringToHash("Dead");
        
        protected string Name;
        protected int Health;
        protected int Damage;

        protected Animator Animator;
        protected Rigidbody2D RigidbodyMonster;

        protected  Collider2D Collider2DAttackRange;
        
        public abstract void Attack(Transform transPlayer);
        public abstract void Hit(int hitDamage, Vector2 posPlayer);
    }
}