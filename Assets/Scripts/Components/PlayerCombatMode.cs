using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public struct PlayerCombatMode : IComponentData
    {
        public bool isCombatMode;
    }
}
