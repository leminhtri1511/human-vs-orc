using HVO.Scripts.Common;
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
            var structureGo = new GameObject(_buildActionSO.ActionName);
            var renderer = structureGo.AddComponent<SpriteRenderer>();

            renderer.sortingOrder = (int)OrderLayer.Structure;
            renderer.sprite = _buildActionSO.FoundationSprite;
            renderer.transform.position = _placementPosition;
        }
    }
}