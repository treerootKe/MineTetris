using System;
using System.Collections;
using Common;
using UnityEngine;
using HollowKnight.AbstractObject;
using HollowKnight.Control;

namespace HollowKnight.SpecialObject.Enemy
{
    public class AttackBehaviourEnemy : AbstractEnemy,IBattleBehaviour
    {
        void Awake()
        {
            base.Awake();
            ItemsName = "attack";
            Health = 3;
            StunDuration = 0.25f;
            MoveSpeed = 1.5f;
            CurrentSpeed = 1.5f;
            AttackingMoveSpeed = 3f;
            IsFly = false;
            DirectionX = (int)transform.localScale.x;
        }

        public void AttackBehaviour(Transform defender)
        {
            CurrentSpeed = AttackingMoveSpeed;
            DirectionX = defender.position.x - transform.position.x > 0 ? 1 : -1;
            DirectionY = IsFly ? (defender.position.y - transform.position.y > 0 ? 1 : -1) : 0;
            transform.localScale = new Vector3(DirectionX, 1, 1);
            AnimatorGameObject.SetTrigger(CommonFields.Attack);
        }

        public void StopAttacking(bool isBeHit)
        {
            if (isBeHit)
            {
                CurrentSpeed = 0;
                IsStunned = true;
                return;
            }
            AnimatorGameObject.SetTrigger(CommonFields.Movement);
            IsStunned = false;
            CurrentSpeed = MoveSpeed;
        }
        
        public override void BeHit(int hitDamage, Vector2 posPlayer)
        {
            StopAttacking(true);
            Health -= hitDamage;
            var backDirection = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            if (Health > 0)
            {
                RigidBodyGameObject.AddForce(new Vector2(backDirection * 10, 2), ForceMode2D.Impulse);
                AnimatorGameObject.SetTrigger(CommonFields.Hit);
                StartCoroutine(WaitStun());
                return;
            }
            RigidBodyGameObject.AddForce(new Vector2(backDirection * 5, 2), ForceMode2D.Impulse);
            AnimatorGameObject.SetTrigger(CommonFields.Dead);
            StartCoroutine(Recycle());
        }

        public IEnumerator WaitStun()
        {
            yield return new WaitForSeconds(StunDuration);
            if (!IsStunned || Health <= 0)
            {
                yield break;
            }
            IsStunned = false;
            AttackBehaviour(PlayerController.Instance.transform);
        }
        
        private IEnumerator Recycle()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            UpdateMovement();
            UpdateMoveX(CurrentSpeed);
            UpdateDirection();
        }

        private void UpdateMovement()
        {
            AnimatorGameObject.SetInteger(CommonFields.Movement, CurrentSpeed != 0 ? 1 : 0);
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
                DirectionX = -DirectionX;
            }
        }
    }
}