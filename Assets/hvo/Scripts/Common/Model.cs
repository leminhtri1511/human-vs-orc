using System;
using HVO.Scripts.ScriptableObjects;

namespace HVO.Scripts.Common
{
    [Serializable]
    public struct ResourceInfo
    {
        public ResourceSO ResourceSO;
        public int Amount;
    }

    public enum GameResourcesType
    {
        Unknown = 0,
        Gold = 1,
        Diamond = 2,
        Wood = 4
    }

    public enum OrderLayer
    {
        Unknown = -99,
        Water = -5,
        Rock = -4,
        Foam = -3,
        Elevations = -2,
        UnderTerrain = -1,
        Walkable = 0,
        Unreachable = 1,
        Structure = 2,
        Unit = 5,
        Pointer = 10,
        Placement = 20,
        AlwaysOnTop = 100
    }

    public enum UnitState
    {
        Unknown = 0,
        Idle = 1,
        Moving = 2,
        Attacking = 3,
        Chopping = 4,
        Mining = 5
    }

    public enum UnitTask
    {
        Unknown = 0,
        Build = 1,
        Chop = 2,
        Mine = 3,
        Attack = 4
    }
}