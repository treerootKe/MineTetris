using System;
using System.Collections;
using Common;
using UnityEngine;
using HollowKnight.AbstractObject;
using HollowKnight.Control;
using HollowKnight.ObjectsBehaviourInterface;

namespace HollowKnight.SpecialObject.Enemy
{
    public class AttackBehaviourEnemy : AbstractEnemy,IEnemiesBehaviour
    {
        new void Awake()
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
        
        public void ChangeDirection()
        {
            DirectionX = -DirectionX;
        }
        
        public  IEnumerator WaitStun()
        {
            yield return new WaitForSeconds(StunDuration);
            if (!IsStunned || Health <= 0)
            {
                yield break;
            }
            IsStunned = false;
            AttackBehaviour(PlayerController.Instance.transform);
        }
        
        public override void BeHit(Vector2 posPlayer,int hitDamage = 0)
        {
            CommonMethod.CameraShake(0.25f);
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
            var beHitAble = collision.gameObject.GetComponent<IDefenseBehaviour>();
            beHitAble?.BeHit(transform.position);
        }
    }
}