using System;
using HollowKnight.AbstractObject;
using HollowKnight.Control;
using UnityEngine;

namespace HollowKnight.SpecialObject.TriggerDetect
{
    public class EnemyAttackTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var trigger = transform.parent.GetComponent<PlayerController>();
            trigger?.AttackBehaviour(other.transform);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var trigger = transform.parent.GetComponent<IEnemiesBehaviour>();
            trigger?.StopAttacking(false);
        }
    }
}