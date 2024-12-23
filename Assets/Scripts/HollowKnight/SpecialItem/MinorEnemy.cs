using System;
using HollowKnight.AbstractClass;
using UnityEngine;

namespace HollowKnight.SpecialItem
{
    public class MinorEnemy: Enemy
    {
        public MinorEnemy()
        {
            name = "minor";
            Health = 3;
            Damage = 1;
        }

        public override void Attack()
        {
            
        }

        public override void Hit()
        {
            
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                
            }
        }
    }
}