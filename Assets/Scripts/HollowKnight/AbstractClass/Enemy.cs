using UnityEngine;

namespace HollowKnight.AbstractClass
{
    public abstract class Enemy: MonoBehaviour
    {
        protected static readonly int Dead = Animator.StringToHash("Dead");
        
        protected string Name;
        protected int Health;
        protected int Damage;

        protected Animator Animator;
        protected Rigidbody2D RigidbodyMonster;

        public abstract void Attack();
        public abstract void Hit(int hitDamage, Vector2 posPlayer);
    }
}