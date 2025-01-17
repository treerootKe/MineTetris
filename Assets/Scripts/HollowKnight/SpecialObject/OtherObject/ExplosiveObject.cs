using System.Collections;
using HollowKnight.AbstractObject;
using UnityEngine;

namespace HollowKnight.SpecialObject.OtherObject
{
    public class ExplosiveObject:AbstractEnemy
    {
        private void Awake()
        {
            itemsName = "attack";
            animatorEnemy = gameObject.GetComponent<Animator>();
            rigidbodyEnemy = transform.GetComponent<Rigidbody2D>();
        }

        
        public override void AttackBehaviour(Transform transPlayer)
        {
        }

        public override void StopAttacking(bool isBeHit)
        {
        }
        
        public override void BeHit(int hitDamage, Vector2 posPlayer)
        {
            health -= hitDamage;
            var direction = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            animatorEnemy.Play("hit");
            rigidbodyEnemy.AddForce(new Vector2(direction * 5, 2), ForceMode2D.Impulse);
            if (health <= 0)
            {
                animatorEnemy.SetBool(Dead, true);
            }
        }
    }
}