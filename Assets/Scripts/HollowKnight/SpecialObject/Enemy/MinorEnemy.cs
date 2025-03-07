using System.Collections;
using Common;
using HollowKnight.AbstractObject;
using UnityEngine;

namespace HollowKnight.SpecialObject.Enemy
{
    public class MinorEnemy: AbstractEnemy
    {
        private  void Awake()
        {
            ItemsName = "minor";
            Health = 3;

        }
        
        public override void BeHit(Vector2 posPlayer,int hitDamage = 0)
        {
            Health -= hitDamage;
            var direction = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            AnimatorGameObject.Play("hit");
            RigidBodyGameObject.AddForce(new Vector2(direction * 5, 2), ForceMode2D.Impulse);
            if (Health > 0)
            {
                return;
            }
            AnimatorGameObject.SetBool(CommonFields.Dead, true);
            StartCoroutine(Recycle());
        }

        protected  void UpdateMovement()
        {
            AnimatorGameObject.SetInteger(CommonFields.Movement, CurrentSpeed != 0 ? 1 : 0);
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