using UnityEngine;

namespace HVO.Scripts.Units
{
    public class WorkerUnit : HumanoidUnit
    {
        protected override void UpdateBehaviour()
        {
            CheckForCloseObjects();
        }

        private void CheckForCloseObjects()
        {
            var hits = RunProximityObjectDetection();

            foreach (var hit in hits)
            {
                if (hit.gameObject == gameObject) continue;

                Debug.Log(hit.gameObject.name);
            }
        }
    }
}