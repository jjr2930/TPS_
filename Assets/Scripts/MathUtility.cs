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

        public static float3 GetSphericalCoordinatesPosition(float radius, float elevation, float polar)    
        {
            //calculate spherical coordinates system
            //wikipidia
            //x = rsin(theta)cos(pi);
            //y = rsin(theta)sin(pi);
            //z = rcos(theta);
            //convert to unity coordinate system
            //y => x, z => y x => -z
            //so..
            //-z = rsin(theta)cos(pi);
            //x = rsin(theta)sin(pi);
            //y = rcos(theta);

            //sin , cos save? to dictionary?
            var a = radius * math.cos(elevation);
            return new float3()
            {                
                x = a * math.sin(polar),
                y = radius * math.sin(elevation),
                z = a * math.cos(polar)
            };
        }
    }
}
