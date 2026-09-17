using HVO.Scripts.AI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HVO.Scripts.Managers
{
    public class TilemapManager : SingletonManager<TilemapManager>
    {
        [Header("Tilemaps")]
        [SerializeField] private Tilemap _walkableTilemap;
        [SerializeField] private Tilemap _overlayTilemap;
        [SerializeField] private Tilemap[] _unreachableTilemaps;

        private PathFinding _pathFinding;

        private void Start()
        {
            var bounds = _walkableTilemap.cellBounds;
            var width = bounds.size.x;
            var height = bounds.size.y;

            _pathFinding = new PathFinding(
                width,
                height
            );
        }

        public bool CanPlaceTile(Vector3Int tilePosition)
        {
            return
                _walkableTilemap.HasTile(tilePosition) &&
                !IsInUnreachableTilemap(tilePosition) &&
                !IsBlockedByGameObject(tilePosition);
        }

        public bool IsInUnreachableTilemap(Vector3Int tilePosition)
        {
            foreach (var tilemap in _unreachableTilemaps)
            {
                if (tilemap.HasTile(tilePosition)) return true;
            }

            return false;
        }

        public bool IsBlockedByGameObject(Vector3Int tilePosition)
        {
            var tileSize = _walkableTilemap.cellSize;
            var colliders = Physics2D.OverlapBoxAll(tilePosition + tileSize / 2, tileSize * 0.5f, 0);

            foreach (var collider in colliders)
            {
                var layer = collider.gameObject.layer;
                if (layer == LayerMask.NameToLayer("Player"))
                {
                    return true;
                }
            }

            return false;
        }

        public void SetTileOverlay(Vector3Int tilePosition, Tile tile)
        {
            _overlayTilemap.SetTile(tilePosition, tile);
        }
    }
}