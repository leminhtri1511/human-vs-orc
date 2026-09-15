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
        private readonly Tilemap _walkableTilemap;
        private readonly Tilemap _overlayTilemap;
        private readonly Tilemap[] _unreachableTilemaps;

        public PlacementProcess(BuildActionSO buildActionSO,
            Tilemap walkableTilemap,
            Tilemap overlayTilemap,
            Tilemap[] unreachableTilemaps)
        {
            _buildActionSO = buildActionSO;
            _walkableTilemap = walkableTilemap;
            _overlayTilemap = overlayTilemap;
            _unreachableTilemaps = unreachableTilemaps;
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

            spriteRenderer.sortingOrder = (int)OrderLayer.PendingPlacement;
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
                tile.color = CanPlaceTile(tilePosition) ? _buildActionSO.ValidColor : _buildActionSO.InvalidColor;
                _overlayTilemap.SetTile(tilePosition, tile);
            }
        }

        private void ClearHighlightTiles()
        {
            if (_highlightPositions == null) return;

            foreach (var tilePosition in _highlightPositions)
            {
                _overlayTilemap.SetTile(tilePosition, null);
            }
        }

        public bool CanPlaceTile(Vector3Int tilePosition)
        {
            return _walkableTilemap.HasTile(tilePosition) &&
                   !IsUnreachableTilemap(tilePosition) &&
                   !IsBlockedByGameObject(tilePosition);
        }

        private bool IsUnreachableTilemap(Vector3Int tilePosition)
        {
            foreach (var tileMap in _unreachableTilemaps)
            {
                if (tileMap.HasTile(tilePosition)) return true;
            }

            return false;
        }

        private bool IsBlockedByGameObject(Vector3Int tilePosition)
        {
            var tileSize = _walkableTilemap.cellSize;
            var colliders = Physics2D.OverlapBoxAll(tilePosition + tileSize / 2, tileSize * 0.9f, 0);

            foreach (var collider in colliders)
            {
                var layer = collider.gameObject.layer;
                if (layer == LayerMask.NameToLayer("Player")) return true;
            }

            return false;
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
                if (!CanPlaceTile(tilePosition)) return false;
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