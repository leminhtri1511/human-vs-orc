using HVO.Scripts.Managers;
using HVO.Scripts.ScriptableObjects;
using UnityEngine;

namespace HVO.Scripts.Utils
{
    public class PlacementProcess
    {
        private readonly BuildActionSO _buildActionSO;
        private GameObject _pendingPlacement;
        private static Vector3 WorldPosition => HvoUtils.InputHoldWorldPosition;

        public PlacementProcess(BuildActionSO buildActionSO)
        {
            _buildActionSO = buildActionSO;
        }

        public void Update()
        {
            HandlePlacementPosition();
        }

        private void HandlePlacementPosition()
        {
            if (WorldPosition == Vector3.zero) return;

            _pendingPlacement.transform.position = HvoUtils.SnapPlacementToGrid(WorldPosition);
        }

        public void ShowPendingPlacement()
        {
            _pendingPlacement = new GameObject("PendingPlacement");

            var spriteRenderer = _pendingPlacement.AddComponent<SpriteRenderer>();

            spriteRenderer.sortingOrder = (int)OrderLayer.PendingPlacement;
            spriteRenderer.color = new Color(1, 1, 1, 0.65f);
            spriteRenderer.sprite = _buildActionSO.PlacementSprite;
        }
    }
}