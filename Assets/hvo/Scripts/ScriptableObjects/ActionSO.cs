using UnityEngine;

namespace HVO.Scripts.ScriptableObjects
{
    public abstract class ActionSO : ScriptableObject
    {
        [Header("Base Info")]
        public Sprite ActionIcon;
        public string ActionName;
        public string Guid = System.Guid.NewGuid().ToString();
    }
}