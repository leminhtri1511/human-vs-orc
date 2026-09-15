using HVO.Scripts.ScriptableObjects;
using UnityEngine;

namespace HVO.Scripts.Utils
{
    public class BuildingProcess
    {
        private readonly BuildActionSO _buildActionSO;
        private readonly Vector3 _placementPosition;

        public BuildingProcess(BuildActionSO buildActionSO, Vector3 placementPosition)
        {
            _buildActionSO = buildActionSO;
            _placementPosition = placementPosition;

            SetupStructure();
        }

        private void SetupStructure()
        {
            var structure = Object.Instantiate(_buildActionSO.StructurePrefab);

            structure.SpriteRenderer.sprite = _buildActionSO.FoundationSprite;
            structure.transform.position = _placementPosition;
        }
    }
}