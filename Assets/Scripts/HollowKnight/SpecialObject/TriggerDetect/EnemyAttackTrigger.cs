using HollowKnight.AbstractClass;
using UnityEngine;

namespace HollowKnight.SpecialObject.TriggerDetect
{
    public class EnemyAttackTrigger : MonoBehaviour, ITriggerEvent
    {
        public void TriggerEvent(Collider2D collider2d)
        {
            transform.parent.GetComponent<AbstractEnemy>().AttackBehaviour(collider2d.transform);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                TriggerEvent(other);
            }
        }
    }
}