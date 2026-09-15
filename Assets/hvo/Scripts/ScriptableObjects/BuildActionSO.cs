using System.Collections.Generic;
using HVO.Scripts.Common;
using UnityEngine;

namespace HVO.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BuildActionSO", menuName = "HVO/Actions/BuildActionSO")]
    public class BuildActionSO : ActionSO
    {
        [Header("Action Sprites")]
        [SerializeField] private Sprite _placementSprite;
        [SerializeField] private Sprite _foundationSprite;
        [SerializeField] private Sprite _completionSprite;
        [SerializeField] private Sprite _overlayPlacementSprite;

        [Header("Vectors")]
        [SerializeField] private Vector3Int _buildingSize;
        [SerializeField] private Vector3Int _originOffset;

        [Header("Required Resources")]
        [SerializeField] private List<ResourceInfo> _requiredResources;

        public Sprite PlacementSprite => _placementSprite;
        public Sprite FoundationSprite => _foundationSprite;
        public Sprite CompletionSprite => _completionSprite;
        public Sprite OverlayPlacementSprite => _overlayPlacementSprite;

        public Vector3Int BuildingSize => _buildingSize;
        public Vector3Int OriginOffset => _originOffset;

        public List<ResourceInfo> RequiredResources => _requiredResources;
    }
}