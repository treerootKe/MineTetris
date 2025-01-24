using System.Collections;
using UnityEngine;

namespace HollowKnight.SpecialObject.Enemy
{
    public class MinorEnemy: AbstractObject.AbstractEnemy
    {
        private  void Awake()
        {
            itemsName = "minor";
            health = 3;
            damage = 1;
            animatorEnemy = gameObject.GetComponent<Animator>();
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
            animatorEnemy.Play("hit");
            rigidbodyGameObject.AddForce(new Vector2(direction * 5, 2), ForceMode2D.Impulse);
            if (health > 0)
            {
                return;
            }
            animatorEnemy.SetBool(Dead, true);
            StartCoroutine(Recycle());
        }

        private IEnumerator Recycle()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                
            }
        }
    }
}