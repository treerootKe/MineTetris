using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Common
{
    public class CommonFields
    {
        public static readonly int Movement = Animator.StringToHash("Movement");
        public static readonly int Grounded = Animator.StringToHash("Grounded");
        public static readonly int SpeedY = Animator.StringToHash("SpeedY");
        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int DoubleJump = Animator.StringToHash("DoubleJump");
        public static readonly int Sliding = Animator.StringToHash("Sliding");
        public static readonly int SlideJump = Animator.StringToHash("SlideJump");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Hit = Animator.StringToHash("Hit");
        public static readonly int Dead = Animator.StringToHash("Dead");
        
        public static List<Tween> S_NeedRecyclesTween = new List<Tween>();
    }
}