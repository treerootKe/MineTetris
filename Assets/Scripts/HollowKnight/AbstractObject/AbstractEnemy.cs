using System;
using System.Collections;
using DesignPattern;
using HollowKnight.Control;
using HollowKnight.ObjectsBehaviourInterface;
using UnityEngine;

namespace HollowKnight.AbstractObject
{
    public abstract class AbstractEnemy : MonoBehaviour,IDefenseBehaviour
    {
        protected string ItemsName;
        protected float AttackingMoveSpeed;
        
        protected float MoveSpeed;
        protected float CurrentSpeed;
        
        protected int Health;
        protected int DirectionX;
        protected int DirectionY;
        protected float StunDuration;
        
        protected bool IsStunned;
        
        protected Animator AnimatorGameObject;
        protected Rigidbody2D RigidBodyGameObject;
        
        protected bool IsFly;

        protected  void Awake()
        {
            AnimatorGameObject = this.GetComponent<Animator>();
            RigidBodyGameObject = this.GetComponent<Rigidbody2D>();
        }

        public float VelocityX
        {
            get => RigidBodyGameObject.velocity.x;
            set => RigidBodyGameObject.velocity = new Vector2(value, VelocityY);
        }

        public float VelocityY
        {
            get => RigidBodyGameObject.velocity.y;
            set => RigidBodyGameObject.velocity = new Vector2(VelocityX, value);
        }
        
        protected void UpdateMoveX(float speed)
        {
            if (CurrentSpeed == 0)
            {
                return;
            }

            VelocityX = speed * DirectionX;
        }

        protected void UpdateMoveY(float speed,float jumpPower = 0)
        {
            if (!IsFly)
            {
                return;
            }
            RigidBodyGameObject.AddForce(new Vector2(0, speed * DirectionY), ForceMode2D.Impulse);
        }

        protected void UpdateDirection()
        {
            transform.localScale = new Vector3(DirectionX, 1, 1);
        }

        public abstract void BeHit(Vector2 posPlayer,int hitDamage = 0);
    }
}