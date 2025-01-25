using System.Collections;
using HollowKnight.AbstractObject;
using UnityEngine;

namespace HollowKnight.SpecialObject.Enemy
{
    public class MinorEnemy: AbstractEnemy
    {
        private  void Awake()
        {
            itemsName = "minor";
            health = 3;

        }
        
        public override void BeHit(int hitDamage, Vector2 posPlayer)
        {
            health -= hitDamage;
            var direction = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            animatorGameObject.Play("hit");
            rigidBodyGameObject.AddForce(new Vector2(direction * 5, 2), ForceMode2D.Impulse);
            if (health > 0)
            {
                return;
            }
            animatorGameObject.SetBool(Dead, true);
            StartCoroutine(Recycle());
        }

        protected override void UpdateMovement()
        {
            animatorGameObject.SetInteger(Movement, currentSpeed != 0 ? 1 : 0);
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