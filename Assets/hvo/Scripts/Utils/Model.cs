using System;
using HVO.Scripts.ScriptableObjects;

namespace HVO.Scripts.Utils
{
    [Serializable]
    public struct RequiredResourceConfig
    {
        public RequiredResourceSO ResourceSO;
        public int RequiredAmount;
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
        Unit = 2,
        Pointer = 10,
        PendingPlacement = 20,
        AlwaysOnTop = 100
    }
}