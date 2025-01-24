using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public interface IObjectsMove
    {
        float VelocityX
        {
            get;
            set;
        }

        float VelocityY
        {
            get;
            set;
        }
        void UpdateMove(float speed);
        void UpdateDirection();
        void UpdateMovement();
        void UpdateJump();
        void UpdateGravityScale();

        void AttackBehaviour(Transform transPlayer);
        
        void BeHit(int hitDamage, Vector2 attackerPosition);
    }
}