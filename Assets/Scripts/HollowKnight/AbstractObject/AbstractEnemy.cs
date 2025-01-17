using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public abstract class AbstractEnemy : MonoBehaviour, IObjectsMove

    {
    protected static readonly int Attack = Animator.StringToHash("Attack");
    protected static readonly int Dead = Animator.StringToHash("Dead");
    protected static readonly int Movement = Animator.StringToHash("Movement");
    protected static readonly int Hit = Animator.StringToHash("Hit");

    protected string itemsName;
    protected int health;
    protected int damage;
    protected int direction;
    protected float stunDuration;
    protected float moveSpeed;
    protected float attackingMoveSpeed;
    protected bool isFly;
    protected bool isStunned;

    protected Vector2 movementDirection;

    protected Animator animatorEnemy;
    protected Rigidbody2D rigidbodyEnemy;

    protected float VelocityX
    {
        get => rigidbodyEnemy.velocity.x;
        set => rigidbodyEnemy.velocity = new Vector2(value, VelocityY);
    }

    protected float VelocityY
    {
        get => rigidbodyEnemy.velocity.y;
        set => rigidbodyEnemy.velocity = new Vector2(VelocityX, value);
    }

    public void UpdateMove(float speed)
    {
        if (movementDirection.x == 0 && movementDirection.y == 0)
        {
            return;
        }

        VelocityX = speed * movementDirection.x;
        if (!isFly)
        {
            return;
        }

        VelocityY = speed * movementDirection.y;
    }

    public void UpdateDirection()
    {
        transform.localScale = new Vector3(direction, 1, 1);
    }

    public void UpdateMovement()
    {

    }

    public void UpdateJump()
    {

    }

    public void UpdateGravityScale()
    {

    }

    public abstract void AttackBehaviour(Transform transPlayer);

    public abstract void StopAttacking(bool isBeHit);

    public abstract void BeHit(int hitDamage, Vector2 posPlayer);
    }
}