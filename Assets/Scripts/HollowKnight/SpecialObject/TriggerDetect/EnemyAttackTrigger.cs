using System;
using HollowKnight.AbstractObject;
using UnityEngine;

namespace HollowKnight.SpecialObject.TriggerDetect
{
    public class EnemyAttackTrigger : MonoBehaviour, ITriggerEvent
    {
        public void TriggerEvent(Collider2D collider2d)
        {
            // (transform.parent.GetComponent<MonoBehaviour>() as IBattleBehaviour)?.AttackBehaviour(collider2d.transform);
            transform.parent.GetComponent<IBattleBehaviour>().AttackBehaviour(collider2d.transform);
        }

        public void TriggerExitEvent(Collider2D collider2d)
        {
            // (transform.parent.GetComponent<MonoBehaviour>() as IBattleBehaviour)?.StopAttacking(false);
            transform.parent.GetComponent<IBattleBehaviour>()?.StopAttacking(false);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                TriggerEvent(other);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                TriggerExitEvent(other);
            }
        }
    }
}