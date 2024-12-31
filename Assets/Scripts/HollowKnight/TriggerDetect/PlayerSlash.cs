using System;
using HollowKnight.AbstractClass;
using UnityEngine;

namespace HollowKnight.TriggerDetect
{
    public class PlayerSlash:MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag($"Enemy") || other.CompareTag($"SpecialEffect"))
            {
                
            }
        }
    }
}