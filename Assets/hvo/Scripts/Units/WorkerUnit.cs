using HVO.Scripts.Common;
using UnityEngine;

namespace HVO.Scripts.Units
{
    public class WorkerUnit : HumanoidUnit
    {
        protected override void UpdateBehaviour()
        {
            if (CurrentTask == UnitTask.Unknown) return;

            CheckForCloseObjects();
        }

        protected override void OnSetDestination()
        {
            ResetState();
        }

        private void CheckForCloseObjects()
        {
            var hits = RunProximityObjectDetection();

            foreach (var hit in hits)
            {
                if (hit.gameObject == gameObject) continue;
                if (CurrentTask != UnitTask.Build || hit.gameObject != Target.gameObject) continue;

                if (hit.TryGetComponent<StructureUnit>(out var structureUnit))
                {
                    StartBuilding(structureUnit);
                }
            }
        }

        private void StartBuilding(StructureUnit structureUnit)
        {
            Debug.Log("Start build" + structureUnit.gameObject.name);
        }

        private void ResetState()
        {
            SetTask(UnitTask.Unknown);

            if (HasTarget)
            {
                CleanupTarget();
            }
        }

        private void CleanupTarget()
        {
            SetTarget(null);
        }
    }
}