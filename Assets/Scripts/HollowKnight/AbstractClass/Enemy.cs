using UnityEngine;

namespace HollowKnight.AbstractClass
{
    public abstract class Enemy: MonoBehaviour
    {
        public string Name;
        public int Health;
        public int Damage;

        public abstract void Attack();
        public abstract void Hit();
    }
}