using HVO.Scripts.Managers;
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

        [Header("Resources Cost")]
        [SerializeField] private int _goldCost;
        [SerializeField] private int _woodCost;

        [Header("Vectors")]
        [SerializeField] private Vector3Int _buildingSize;
        [SerializeField] private Vector3Int _originOffset;

        [Header("Colors")]
        [SerializeField] private Color _validColor;
        [SerializeField] private Color _invalidColor;

        public Sprite PlacementSprite => _placementSprite;
        public Sprite FoundationSprite => _foundationSprite;
        public Sprite CompletionSprite => _completionSprite;
        public Sprite OverlayPlacementSprite => _overlayPlacementSprite;

        public int GoldCost => _goldCost;
        public int WoodCost => _woodCost;

        public Vector3Int BuildingSize => _buildingSize;
        public Vector3Int OriginOffset => _originOffset;

        public Color ValidColor => _validColor;
        public Color InvalidColor => _invalidColor;

        public override void Execute(GameManager gameManager)
        {
            gameManager.StartBuildProgress(this);
        }
    }
}