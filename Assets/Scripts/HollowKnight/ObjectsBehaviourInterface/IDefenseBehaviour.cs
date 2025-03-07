using UnityEngine;

namespace HollowKnight.ObjectsBehaviourInterface
{
    public interface IDefenseBehaviour
    {
        void BeHit(Vector2 attackerPosition, int damage = 0);
    }
}