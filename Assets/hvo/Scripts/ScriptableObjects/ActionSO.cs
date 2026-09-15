using UnityEngine;

namespace HVO.Scripts.ScriptableObjects
{
    public abstract class ActionSO : ScriptableObject
    {
        [Header("Base Info")]
        public Sprite ActionIcon;
        public string ActionName;
        public string Guid = System.Guid.NewGuid().ToString();
        public Color32 ValidColor = new Color32(25, 255, 0, 123);
        public Color32 InvalidColor = new Color32(255, 34, 0, 128);
    }
}