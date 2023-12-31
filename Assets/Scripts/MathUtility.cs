using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MyTPS
{
    public static class MathUtility
    {
        public static float MoveToward(float from, float to, float maxDelta)
        {
            return math.abs(to - from) < maxDelta
                ? to
                : from + math.sign(to - from) * maxDelta;
        }
    }
}
