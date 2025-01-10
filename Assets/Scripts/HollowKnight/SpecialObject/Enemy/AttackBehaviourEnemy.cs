using System;
using System.Collections;
using UnityEngine;
using HollowKnight.AbstractObject;
using HollowKnight.Control;

namespace HollowKnight.SpecialObject.Enemy
{
    public class AttackBehaviourEnemy : AbstractEnemy
    {
        private float speed = 1.5f;
        
        private void Awake()
        {
            Name = "attack";
            Health = 3;
            Damage = 1;
            StunDuration = 0.25f;
            MoveSpeed = 1.5f;
            AttackingMoveSpeed = 3f;
            MovementDirection = new Vector2(transform.localScale.x, 0);
            Direction = (int)MovementDirection.x;
            AnimatorEnemy = gameObject.GetComponent<Animator>();
            RigidbodyEnemy = transform.GetComponent<Rigidbody2D>();
        }

        public override void AttackBehaviour(Transform transPlayer)
        {
            speed = AttackingMoveSpeed;
            var directionX = transPlayer.position.x - transform.position.x > 0 ? 1 : -1;
            var directionY = IsFly ? (transPlayer.position.y - transform.position.y > 0 ? 1 : -1) : 0;
            transform.localScale = new Vector3(directionX, 1, 1);
            AnimatorEnemy.SetTrigger(Attack);
            MovementDirection = new Vector2(directionX, directionY);
        }

        public override void StopAttacking(bool isBeHit)
        {
            if (isBeHit)
            {
                MovementDirection = Vector2.zero;
                IsStunned = true;
                return;
            }
            AnimatorEnemy.SetTrigger(Movement);
            IsStunned = false;
            speed = MoveSpeed;
        }
        
        public override void BeHit(int hitDamage, Vector2 posPlayer)
        {
            StopAttacking(true);
            Health -= hitDamage;
            var direction = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            RigidbodyEnemy.AddForce(new Vector2(direction * 5, 2), ForceMode2D.Impulse);
            if (Health > 0)
            {
                AnimatorEnemy.SetTrigger(Hit);
                StartCoroutine(WaitStun());
                return;
            }
            AnimatorEnemy.SetTrigger(Dead);
            StartCoroutine(Recycle());
        }

        private IEnumerator WaitStun()
        {
            yield return new WaitForSeconds(StunDuration);
            if (!IsStunned)
            {
                yield break;
            }
            IsStunned = false;
            AttackBehaviour(PlayerController.Instance.transform);
        }
        
        private IEnumerator Recycle()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            UpdateMove(speed);
            UpdateDirection();
        }
        
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var normal = collision.contacts[0].normal;
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
                VelocityX = 0;
                VelocityY = 0;
                Direction = -Direction;
            }
        }
    }
}