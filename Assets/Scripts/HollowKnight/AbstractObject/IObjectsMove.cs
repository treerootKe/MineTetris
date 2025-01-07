using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public interface IObjectsMove
    {
        void UpdateMove(float speed);
        void UpdateDirection();
        void UpdateMovement();
        void UpdateJump();
        void UpdateGravityScale();
    }
}