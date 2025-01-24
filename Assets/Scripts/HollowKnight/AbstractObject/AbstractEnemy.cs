using DesignPattern;
using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public abstract class AbstractEnemy : AbstractSameMovement
    {
        protected static readonly int Attack = Animator.StringToHash("Attack");
        protected static readonly int Movement = Animator.StringToHash("Movement");
        protected static readonly int Hit = Animator.StringToHash("Hit");

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

        protected override void UpdateMoveY(float speed)
        {
            if (!isFly)
            {
                return;
            }
            rigidbodyGameObject.AddForce(new Vector2(0, speed * directionY), ForceMode2D.Impulse);
        }

        protected override void UpdateDirection()
        {
            transform.localScale = new Vector3(directionX, 1, 1);
        }
        
    }
}