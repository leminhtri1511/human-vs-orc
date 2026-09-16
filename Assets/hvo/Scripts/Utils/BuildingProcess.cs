using HVO.Scripts.Common;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.Units;
using UnityEngine;

namespace HVO.Scripts.Utils
{
    public class BuildingProcess
    {
        private readonly BuildActionSO _buildActionSO;
        private readonly Vector3 _placementPosition;
        private WorkerUnit _worker;

        public BuildingProcess(BuildActionSO buildActionSO, Vector3 placementPosition, WorkerUnit worker)
        {
            _buildActionSO = buildActionSO;
            _placementPosition = placementPosition;
            _worker = worker;

            SetupStructure();
        }

        private void SetupStructure()
        {
            var structure = Object.Instantiate(_buildActionSO.StructurePrefab);

            structure.SpriteRenderer.sprite = _buildActionSO.FoundationSprite;
            structure.transform.position = _placementPosition;
            structure.RegisterProcess(this);

            _worker.MoveTo(_placementPosition);
            _worker.SetTask(UnitTask.Build);
            _worker.SetTarget(structure);
        }

        public void Update()
        {
            Debug.Log("UNDER CONSTRUCTION");
        }
    }
}