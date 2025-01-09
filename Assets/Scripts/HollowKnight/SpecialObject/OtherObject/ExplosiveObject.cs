using System.Collections;
using HollowKnight.AbstractObject;
using UnityEngine;

namespace HollowKnight.SpecialObject.OtherObject
{
    public class ExplosiveObject:AbstractEnemy
    {
        private void Awake()
        {
            Name = "attack";
            AnimatorEnemy = gameObject.GetComponent<Animator>();
            RigidbodyEnemy = transform.GetComponent<Rigidbody2D>();
        }

        
        public override void AttackBehaviour(Transform transPlayer)
        {
        }

        public override void StopAttacking(bool isBeHit)
        {
        }
        
        public override void BeHit(int hitDamage, Vector2 posPlayer)
        {
            Health -= hitDamage;
            var direction = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            AnimatorEnemy.Play("hit");
            RigidbodyEnemy.AddForce(new Vector2(direction * 5, 2), ForceMode2D.Impulse);
            if (Health <= 0)
            {
                AnimatorEnemy.SetBool(Dead, true);
            }
        }
    }
}