using System;
using System.Collections;
using UnityEngine;
using HollowKnight.AbstractObject;
using HollowKnight.Control;

namespace HollowKnight.SpecialObject.Enemy
{
    public class AttackBehaviourEnemy : AbstractEnemy
    {
        private float _speed = 1.5f;
        
        private void Awake()
        {
            itemsName = "attack";
            health = 3;
            damage = 1;
            stunDuration = 0.25f;
            moveSpeed = 1.5f;
            attackingMoveSpeed = 3f;
            isFly = false;
            movementDirection = new Vector2(transform.localScale.x, 0);
            direction = (int)movementDirection.x;
            animatorEnemy = gameObject.GetComponent<Animator>();
            rigidbodyEnemy = transform.GetComponent<Rigidbody2D>();
        }

        public override void AttackBehaviour(Transform transPlayer)
        {
            _speed = attackingMoveSpeed;
            var directionX = transPlayer.position.x - transform.position.x > 0 ? 1 : -1;
            var directionY = isFly ? (transPlayer.position.y - transform.position.y > 0 ? 1 : -1) : 0;
            direction = directionX;
            transform.localScale = new Vector3(directionX, 1, 1);
            animatorEnemy.SetTrigger(Attack);
            movementDirection = new Vector2(directionX, directionY);
        }

        public override void StopAttacking(bool isBeHit)
        {
            if (isBeHit)
            {
                movementDirection = Vector2.zero;
                isStunned = true;
                return;
            }
            animatorEnemy.SetTrigger(Movement);
            isStunned = false;
            _speed = moveSpeed;
        }
        
        public override void BeHit(int hitDamage, Vector2 posPlayer)
        {
            StopAttacking(true);
            health -= hitDamage;
            var backDirection = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            if (health > 0)
            {
                rigidbodyEnemy.AddForce(new Vector2(backDirection * 10, 2), ForceMode2D.Impulse);
                animatorEnemy.SetTrigger(Hit);
                StartCoroutine(WaitStun());
                return;
            }
            rigidbodyEnemy.AddForce(new Vector2(backDirection * 5, 2), ForceMode2D.Impulse);
            animatorEnemy.SetTrigger(Dead);
            StartCoroutine(Recycle());
        }

        private IEnumerator WaitStun()
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
            UpdateMove(_speed);
            UpdateDirection();
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
                direction = -direction;
                movementDirection = new Vector2(direction, movementDirection.y);
            }
        }
    }
}