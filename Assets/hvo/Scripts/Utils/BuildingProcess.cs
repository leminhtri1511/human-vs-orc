using HVO.Scripts.Common;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.Units;
using UnityEngine;

namespace HVO.Scripts.Utils
{
    public class BuildingProcess
    {
        private WorkerUnit _worker;

        public bool HasActiveWorker => _worker != null;

        public BuildingProcess(BuildActionSO buildActionSO, Vector3 placementPosition, WorkerUnit worker)
        {
            ProcessHandling(buildActionSO, placementPosition, worker);
        }

        private void ProcessHandling(BuildActionSO buildActionSO, Vector3 placementPosition, WorkerUnit worker)
        {
            var structure = Object.Instantiate(buildActionSO.StructurePrefab);

            structure.SpriteRenderer.sprite = buildActionSO.FoundationSprite;
            structure.transform.position = placementPosition;
            structure.RegisterProcess(this);

            worker.MoveTo(placementPosition);
            worker.SetTask(UnitTask.Build);
            worker.SetTarget(structure);
        }

        public void Update()
        {
            // if (HasActiveWorker)
            // {
                // Debug.Log("UNDER CONSTRUCTION");
            // }
        }

        public void AddWorker(WorkerUnit worker)
        {
            if (HasActiveWorker) return;

            _worker = worker;
        }

        public void RemoveWorker()
        {
            if (!HasActiveWorker) return;

            _worker = null;
        }
    }
}