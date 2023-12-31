using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTPS
{
    public static class AnimatorHash 
    {
        public static int X = Animator.StringToHash("X");
        public static int Z = Animator.StringToHash("Z");
        public static int Ground = Animator.StringToHash("Ground");
        public static int Falling = Animator.StringToHash("Falling");
        public static int Jump = Animator.StringToHash("Jump");
        public static int Locomotion = Animator.StringToHash("Locomotion");
        public static int Moving = Animator.StringToHash("Moving");
    }
}
