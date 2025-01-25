using System.Collections;
using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public interface IBattleBehaviour
    {
        void AttackBehaviour(Transform defender);

        void StopAttacking(bool isBeHit);

        IEnumerator WaitStun();
    }
}