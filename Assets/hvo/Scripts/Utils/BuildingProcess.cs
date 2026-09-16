using HVO.Scripts.Common;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.Units;
using UnityEngine;

namespace HVO.Scripts.Utils
{
    public class BuildingProcess
    {
        private BuildActionSO _buildActionSO;
        private WorkerUnit _workerUnit;
        private StructureUnit _structureUnit;
        private float _processTimer;
        private bool _isBuildingFinished;
        public bool IsUnderConstruction => HasActiveWorker && _workerUnit.CurrentState == UnitState.Building;

        public bool HasActiveWorker => _workerUnit != null;

        public BuildingProcess(BuildActionSO buildActionSO, Vector3 placementPosition, WorkerUnit worker)
        {
            _buildActionSO = buildActionSO;

            ProcessHandling(buildActionSO, placementPosition, worker);
        }

        private void ProcessHandling(BuildActionSO buildActionSO, Vector3 placementPosition, WorkerUnit worker)
        {
            _structureUnit = Object.Instantiate(buildActionSO.StructurePrefab);
            _structureUnit.SpriteRenderer.sprite = buildActionSO.FoundationSprite;
            _structureUnit.transform.position = placementPosition;
            _structureUnit.RegisterProcess(this);

            worker.SendToBuild(_structureUnit);
        }

        public void Update()
        {
            if (_isBuildingFinished) return;

            if (!IsUnderConstruction) return;

            _processTimer += Time.deltaTime;

            if ((_processTimer >= _buildActionSO.ConstructionTime))
            {
                HandleBuildingFinished();
            }
        }

        private void HandleBuildingFinished()
        {
            _isBuildingFinished = true;
            _structureUnit.SpriteRenderer.sprite = _buildActionSO.CompletionSprite;
            _workerUnit.OnBuildingFinished();
            _structureUnit.OnConstructionFinished();
        }

        public void AddWorker(WorkerUnit worker)
        {
            if (HasActiveWorker) return;

            _workerUnit = worker;
        }

        public void RemoveWorker()
        {
            if (!HasActiveWorker) return;

            _workerUnit = null;
        }
    }
}