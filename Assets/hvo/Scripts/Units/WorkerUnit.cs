using HVO.Scripts.Common;
using UnityEngine;

namespace HVO.Scripts.Units
{
    public class WorkerUnit : HumanoidUnit
    {
        private static readonly int IsBuildHash = Animator.StringToHash("IsBuilding");

        protected override void UpdateBehaviour()
        {
            if (CurrentTask == UnitTask.Build && HasTarget)
            {
                CheckForConstruction();
            }
        }

        protected override void OnSetDestination()
        {
            ResetState();
        }

        public void OnBuildingFinished()
        {
            ResetState();
        }

        public void SendToBuild(StructureUnit structureUnit)
        {
            MoveTo(structureUnit.transform.position);
            SetTarget(structureUnit);
            SetTask(UnitTask.Build);
        }

        private void StartBuilding(StructureUnit structureUnit)
        {
            SetState(UnitState.Building);
            UnitAnimator.SetBool(IsBuildHash, true);
            structureUnit.AssignWorker(this);
        }

        private void ResetState()
        {
            SetTask(UnitTask.Unknown);

            if (HasTarget)
            {
                CleanupTarget();
            }

            UnitAnimator.SetBool(IsBuildHash, false);
        }

        private void CleanupTarget()
        {
            if (Target is StructureUnit structureUnit)
            {
                structureUnit.UnassignWorker();
            }

            SetTarget(null);
        }

        private void CheckForConstruction()
        {
            var distanceToConstruction = Vector3.Distance(transform.position, Target.transform.position);

            if (distanceToConstruction <= ObjectDetectionRadius)
            {
                StartBuilding(Target as StructureUnit);
            }
        }

        // private void CheckForCloseObjects()
        // {
        //     var hits = RunProximityObjectDetection();
        //
        //     foreach (var hit in hits)
        //     {
        //         if (hit.gameObject == gameObject) continue;
        //         if (CurrentTask != UnitTask.Build || hit.gameObject != Target.gameObject) continue;
        //
        //         if (hit.TryGetComponent<StructureUnit>(out var structureUnit))
        //         {
        //             StartBuilding(structureUnit);
        //         }
        //     }
        // }
    }
}