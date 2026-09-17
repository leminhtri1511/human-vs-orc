using HVO.Scripts.Common;
using HVO.Scripts.Managers;
using HVO.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HVO.Scripts.Utils
{
    public class PlacementProcess
    {
        private GameObject _pendingPlacement;
        private readonly BuildActionSO _buildActionSO;
        private Vector3Int[] _highlightPositions;
        private TilemapManager _tilemapManager;

        public PlacementProcess(BuildActionSO buildActionSO,
            TilemapManager tilemapManager)
        {
            _buildActionSO = buildActionSO;
            _tilemapManager = tilemapManager;
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
            if (HvoUtils.IsPointerOverUIElement()) return;

            if (HvoUtils.TryGetHoldPosition(out var worldPosition))
                _pendingPlacement.transform.position = HvoUtils.SnapPlacementToGrid(worldPosition);
        }

        public void ShowPendingPlacement()
        {
            _pendingPlacement = new GameObject("PendingPlacement");

            var spriteRenderer = _pendingPlacement.AddComponent<SpriteRenderer>();

            spriteRenderer.sortingOrder = (int)OrderLayer.Placement;
            spriteRenderer.color = new Color(1, 1, 1, 0.65f);
            spriteRenderer.sprite = _buildActionSO.PlacementSprite;
        }

        private void HighlightTiles(Vector3 outlinePosition)
        {
            var buildingSize = _buildActionSO.BuildingSize;
            var correctPosition = outlinePosition + _buildActionSO.OriginOffset;

            ClearHighlightTiles();
            _highlightPositions = new Vector3Int[buildingSize.x * buildingSize.y];

            for (int x = 0; x < buildingSize.x; x++)
            {
                for (int y = 0; y < buildingSize.y; y++)
                {
                    _highlightPositions[x + y * buildingSize.x] =
                        new Vector3Int((int)correctPosition.x + x, (int)correctPosition.y + y, 0);
                }
            }

            foreach (var tilePosition in _highlightPositions)
            {
                var tile = ScriptableObject.CreateInstance<Tile>();

                tile.sprite = _buildActionSO.OverlayPlacementSprite;
                tile.color = _tilemapManager.CanPlaceTile(tilePosition)
                    ? _buildActionSO.ValidColor
                    : _buildActionSO.InvalidColor;
                _tilemapManager.SetTileOverlay(tilePosition, tile);
            }
        }

        private void ClearHighlightTiles()
        {
            if (_highlightPositions == null) return;

            foreach (var tilePosition in _highlightPositions)
            {
                _tilemapManager.SetTileOverlay(tilePosition, null);
            }
        }

        public bool TryFinalizePlacement(out Vector3 placementPosition)
        {
            if (IsPlacementAreaValid())
            {
                ClearHighlightTiles();
                placementPosition = _pendingPlacement.transform.position;
                Object.Destroy(_pendingPlacement);
                return true;
            }

            placementPosition = Vector3.zero;
            return false;
        }

        private bool IsPlacementAreaValid()
        {
            foreach (var tilePosition in _highlightPositions)
            {
                if (!_tilemapManager.CanPlaceTile(tilePosition)) return false;
            }

            return true;
        }

        public void ClearPendingPlacement()
        {
            ClearHighlightTiles();
            Object.Destroy(_pendingPlacement);
        }
    }
}