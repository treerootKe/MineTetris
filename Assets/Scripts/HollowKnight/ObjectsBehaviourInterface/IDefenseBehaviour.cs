using UnityEngine;

namespace HollowKnight.ObjectsBehaviourInterface
{
    public interface IDefenseBehaviour
    {
        void BeHit(int damage, Vector2 attackerPosition);
    }
}