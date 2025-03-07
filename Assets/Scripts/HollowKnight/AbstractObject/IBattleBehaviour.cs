using System.Collections;
using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public interface IEnemiesBehaviour
    {
        void AttackBehaviour(Transform defender);

        void StopAttacking(bool isBeHit);

        void ChangeDirection();
        
        IEnumerator WaitStun();
    }
}