using System.Collections;
using UnityEngine;
using HollowKnight.AbstractClass;

namespace HollowKnight.SpecialObject.Enemy
{
    public class AttackBehaviourEnemy : AbstractEnemy
    {
        private void Awake()
        {
            Name = "attack";
            Health = 3;
            Damage = 1;
            Speed = 1.5f;
            AttackSpeed = 3f;
            AnimatorEnemy = gameObject.GetComponent<Animator>();
            RigidbodyEnemy = transform.GetComponent<Rigidbody2D>();

            Collider2DAttackRange = transform.Find("AttackRange").GetComponent<Collider2D>();
        }

        public override void AttackBehaviour(Transform transPlayer)
        {
            var direction = transPlayer.position.x - transform.position.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(direction, 1, 1);
            AnimatorEnemy.SetBool(Attack, true);
            VelocityX = AttackSpeed * direction;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AttackBehaviour(other.transform);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
            }
        }
    }
}