using System.Collections;
using UnityEngine;

namespace HollowKnight.SpecialObject.Enemy
{
    public class MinorEnemy: AbstractObject.AbstractEnemy
    {
        private  void Awake()
        {
            Name = "minor";
            Health = 3;
            Damage = 1;
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
            if (Health > 0)
            {
                return;
            }
            AnimatorEnemy.SetBool(Dead, true);
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