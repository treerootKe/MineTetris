using DesignPattern;
using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public abstract class AbstractEnemy : AbstractSameMovement
    {
        protected static readonly int Attack = Animator.StringToHash("Attack");

        protected string itemsName;
        protected float attackingMoveSpeed;
        
        protected bool isFly;

        protected override void UpdateMoveX(float speed)
        {
            if (currentSpeed == 0)
            {
                return;
            }

            VelocityX = speed * directionX;
        }

        protected override void UpdateMoveY(float speed,float jumpPower = 0)
        {
            if (!isFly)
            {
                return;
            }
            rigidBodyGameObject.AddForce(new Vector2(0, speed * directionY), ForceMode2D.Impulse);
        }

        protected override void UpdateDirection()
        {
            transform.localScale = new Vector3(directionX, 1, 1);
        }
        
    }
}