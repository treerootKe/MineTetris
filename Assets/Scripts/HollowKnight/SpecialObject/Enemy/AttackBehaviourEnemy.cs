using System;
using System.Collections;
using UnityEngine;
using HollowKnight.AbstractObject;

namespace HollowKnight.SpecialObject.Enemy
{
    public class AttackBehaviourEnemy : AbstractEnemy
    {
        private void Awake()
        {
            Name = "attack";
            Health = 3;
            Damage = 1;
            MoveSpeed = 1.5f;
            AttackingMoveSpeed = 3f;
            AnimatorEnemy = gameObject.GetComponent<Animator>();
            RigidbodyEnemy = transform.GetComponent<Rigidbody2D>();
        }

        public override void AttackBehaviour(Transform transPlayer)
        {
            var directionX = transPlayer.position.x - transform.position.x > 0 ? 1 : -1;
            var directionY = IsFly ? 0 : transPlayer.position.y - transform.position.y > 0 ? 1 : -1;
            transform.localScale = new Vector3(directionX, 1, 1);
            AnimatorEnemy.SetTrigger(Attack);
            MovementDirection = new Vector2(directionX, directionY);
        }

        public override void StopAttacking()
        {
            AnimatorEnemy.SetTrigger(Movement);
            MovementDirection = Vector2.zero;
        }
        
        public override void BeHit(int hitDamage, Vector2 posPlayer)
        {
            Health -= hitDamage;
            var direction = posPlayer.x - transform.position.x > 0 ? -1 : 1;
            AnimatorEnemy.Play("hit");
            RigidbodyEnemy.AddForce(new Vector2(direction * 5, 2), ForceMode2D.Impulse);
            if (Health <= 0)
            {
                AnimatorEnemy.SetBool(Dead, true);
                StartCoroutine(Recycle());
            }
        }

        private IEnumerator Recycle()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            UpdateMove(MoveSpeed);
        }
        
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
            }
        }
    }
}