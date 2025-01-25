using System;
using System.Collections;
using UnityEngine;
using HollowKnight.AbstractObject;
using HollowKnight.Control;

namespace HollowKnight.SpecialObject.Enemy
{
    public class AttackBehaviourEnemy : AbstractEnemy,IBattleBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            itemsName = "attack";
            health = 3;
            stunDuration = 0.25f;
            moveSpeed = 1.5f;
            currentSpeed = 1.5f;
            attackingMoveSpeed = 3f;
            isFly = false;
            directionX = (int)transform.localScale.x;
        }

        public void AttackBehaviour(Transform defender)
        {
            currentSpeed = attackingMoveSpeed;
            directionX = defender.position.x - transform.position.x > 0 ? 1 : -1;
            directionY = isFly ? (defender.position.y - transform.position.y > 0 ? 1 : -1) : 0;
            transform.localScale = new Vector3(directionX, 1, 1);
            animatorGameObject.SetTrigger(Attack);
        }

        public void StopAttacking(bool isBeHit)
        {
            if (isBeHit)
            {
                currentSpeed = 0;
                isStunned = true;
                return;
            }
            animatorGameObject.SetTrigger(Movement);
            isStunned = false;
            currentSpeed = moveSpeed;
        }
        
        public override void BeHit(int hitDamage, Vector2 posPlayer)
        {
            StopAttacking(true);
            health -= hitDamage;
            var backDirection = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            if (health > 0)
            {
                rigidBodyGameObject.AddForce(new Vector2(backDirection * 10, 2), ForceMode2D.Impulse);
                animatorGameObject.SetTrigger(Hit);
                StartCoroutine(WaitStun());
                return;
            }
            rigidBodyGameObject.AddForce(new Vector2(backDirection * 5, 2), ForceMode2D.Impulse);
            animatorGameObject.SetTrigger(Dead);
            StartCoroutine(Recycle());
        }

        public IEnumerator WaitStun()
        {
            yield return new WaitForSeconds(stunDuration);
            if (!isStunned || health <= 0)
            {
                yield break;
            }
            isStunned = false;
            AttackBehaviour(PlayerController.Instance.transform);
        }
        
        private IEnumerator Recycle()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            UpdateMoveX(currentSpeed);
            UpdateDirection();
        }

        protected override void UpdateMovement()
        {
            animatorGameObject.SetInteger(Movement, currentSpeed != 0 ? 1 : 0);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var normal = collision.contacts[0].normal;
            if (collision.gameObject.CompareTag("Player"))
            {
                
            }
            
            if ((normal == Vector2.left || normal == Vector2.right))
            {
                if (transform.localScale.x > 0 && normal.x > 0)
                {
                    return;
                }

                if (transform.localScale.x < 0 && normal.x < 0)
                {
                    return;
                }

                // VelocityX = 0;
                // VelocityY = 0;
                directionX = -directionX;
            }
        }
    }
}