using HollowKnight.AbstractObject;
using UnityEngine;

namespace HollowKnight.SpecialObject.TriggerDetect
{
    public class EnemyTurntableTrigger:MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var trigger = other.GetComponent<IEnemiesBehaviour>();
            trigger?.ChangeDirection();
        }
    }
}