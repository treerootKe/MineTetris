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
            animatorGameObject = gameObject.GetComponent<Animator>();
            rigidbodyGameObject = transform.GetComponent<Rigidbody2D>();
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
            animatorGameObject.Play("hit");
            rigidbodyGameObject.AddForce(new Vector2(direction * 5, 2), ForceMode2D.Impulse);
            if (health <= 0)
            {
                animatorGameObject.SetBool(Dead, true);
            }
        }
    }
}