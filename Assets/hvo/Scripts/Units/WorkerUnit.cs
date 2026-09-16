using HVO.Scripts.Common;

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

        public void SendToBuild(StructureUnit structureUnit)
        {
            MoveTo(structureUnit.transform.position);
            SetTarget(structureUnit);
            SetTask(UnitTask.Build);
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
            structureUnit.AssignWorker(this);
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
            if (Target is StructureUnit structureUnit)
            {
                structureUnit.UnassignWorker();
            }

            SetTarget(null);
        }
    }
}