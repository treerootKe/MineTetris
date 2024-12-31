using System;
using System.Collections;
using HollowKnight.AbstractClass;
using UnityEngine;

namespace HollowKnight.SpecialItem
{
    public class MinorEnemy: Enemy
    {
        private void Awake()
        {
            name = "minor";
            Health = 3;
            Damage = 1;
            Animator = gameObject.GetComponent<Animator>();
            RigidbodyMonster = transform.GetComponent<Rigidbody2D>();
        }

        public override void Attack()
        {
            
        }

        public override void Hit(int hitDamage, Vector2 posPlayer)
        {
            Health -= 2;
            var direction = (posPlayer.x - transform.position.x) > 0 ? -1 : 1;
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