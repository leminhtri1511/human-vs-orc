using HVO.Scripts.Managers;
using HVO.Scripts.ScriptableObjects;
using UnityEngine;

namespace HVO.Scripts.Utils
{
    public class PlacementProcess
    {
        private GameObject _pendingPlacement;
        private readonly BuildActionSO _buildActionSO;
        private Vector3Int[] _highlightPositions;

        public PlacementProcess(BuildActionSO buildActionSO)
        {
            _buildActionSO = buildActionSO;
        }

        public void Update()
        {
            HandlePlacementOutline();

            HandlePlacementPosition();
        }

        private void HandlePlacementOutline()
        {
            if (_pendingPlacement != null)
            {
                HighlightTiles(_pendingPlacement.transform.position);
            }
        }

        private void HandlePlacementPosition()
        {
            if (HvoUtils.TryGetHoldPosition(out var worldPosition))
                _pendingPlacement.transform.position = HvoUtils.SnapPlacementToGrid(worldPosition);
        }

        public void ShowPendingPlacement()
        {
            _pendingPlacement = new GameObject("PendingPlacement");

            var spriteRenderer = _pendingPlacement.AddComponent<SpriteRenderer>();

            spriteRenderer.sortingOrder = (int)OrderLayer.PendingPlacement;
            spriteRenderer.color = new Color(1, 1, 1, 0.65f);
            spriteRenderer.sprite = _buildActionSO.PlacementSprite;
        }

        private void HighlightTiles(Vector3 outlinePosition)
        {
            var buildingSize = new Vector2Int(2, 3);
            _highlightPositions = new Vector3Int[buildingSize.x * buildingSize.y];

            for (int x = 0; x < buildingSize.x; x++)
            {
                for (int y = 0; y < buildingSize.y; y++)
                {
                    _highlightPositions[x + y * buildingSize.x] =
                        new Vector3Int((int)outlinePosition.x + x, (int)outlinePosition.y + y, 0);
                }
            }

            foreach (var tilePosition in _highlightPositions)
            {
            }
        }
    }
}