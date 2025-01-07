using System;
using HollowKnight.AbstractObject;
using UnityEngine;

namespace HollowKnight.SpecialObject.TriggerDetect
{
    public class EnemyAttackTrigger : MonoBehaviour, ITriggerEvent
    {
        public void TriggerEvent(Collider2D collider2d)
        {
            transform.parent.GetComponent<AbstractEnemy>().AttackBehaviour(collider2d.transform);
        }

        public void TriggerExitEvent(Collider2D collider2d)
        {
            transform.parent.GetComponent<AbstractEnemy>().StopAttacking();
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