using System.Collections;
using UnityEngine;

namespace HollowKnight.SpecialObject.Enemy
{
    public class MinorAbstractEnemy: AbstractClass.AbstractEnemy
    {
        private  void Awake()
        {
            Name = "minor";
            Health = 3;
            Damage = 1;
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
                StartCoroutine(Recycle());
            }
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