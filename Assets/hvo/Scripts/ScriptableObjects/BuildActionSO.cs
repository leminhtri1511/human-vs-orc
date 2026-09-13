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

        [Header("Resources Cost")]
        [SerializeField] private int _goldCost;
        [SerializeField] private int _woodCost;

        public Sprite PlacementSprite => _placementSprite;
        public Sprite FoundationSprite => _foundationSprite;
        public Sprite CompletionSprite => _completionSprite;

        public int GoldCost => _goldCost;
        public int WoodCost => _woodCost;

        public override void Execute(GameManager gameManager)
        {
            gameManager.StartBuildProgress(this);
        }
    }
}