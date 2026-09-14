using HVO.Scripts.Utils;
using UnityEngine;

namespace HVO.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RequiredResourceSO", menuName = "HVO/Resources/RequiredResourceSO")]
    public class RequiredResourceSO : ScriptableObject
    {
        [Header("Configs")]
        [SerializeField] private GameResourcesType _resourceType = GameResourcesType.Unknown;
        [SerializeField] private Sprite _sprite;

        public GameResourcesType ResourceType => _resourceType;
        public Sprite Sprite => _sprite;
    }
}