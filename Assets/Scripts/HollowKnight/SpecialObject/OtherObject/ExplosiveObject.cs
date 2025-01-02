using System.Collections;
using HollowKnight.AbstractClass;
using UnityEngine;

namespace HollowKnight.SpecialObject.OtherObject
{
    public class ExplosiveObject:AbstractEnemy
    {
        private void Awake()
        {
            Name = "attack";
            Animator = gameObject.GetComponent<Animator>();
            RigidbodyMonster = transform.GetComponent<Rigidbody2D>();
        }

        public override void Attack(Transform transPlayer)
        {
        }

        public override void Hit(int hitDamage, Vector2 posPlayer)
        {
            Health -= hitDamage;
            var direction = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            Animator.Play("hit");
            RigidbodyMonster.AddForce(new Vector2(direction * 5, 2), ForceMode2D.Impulse);
            if (Health <= 0)
            {
                Animator.SetBool(Dead, true);
            }
        }
    }
}