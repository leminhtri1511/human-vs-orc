using HVO.Scripts.Common;
using UnityEngine;

namespace HVO.Scripts.Units
{
    public class HumanoidUnit : Unit
    {
        protected Vector2 Velocity;
        protected Vector3 LastPosition;

        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

        private void OnEnable()
        {
            LastPosition = transform.position;
        }

        private void Update()
        {
            UpdateVelocity();
            UpdateBehaviour();
        }

        protected virtual void UpdateBehaviour()
        {
        }

        protected virtual void UpdateVelocity()
        {
            var positionDelta = transform.position - LastPosition;

            Velocity = new Vector2(positionDelta.x, positionDelta.y) / Time.deltaTime;
            LastPosition = transform.position;

            var state = Velocity.magnitude > 0 ? UnitState.Moving : UnitState.Idle;

            SetState(state);
            UnitAnimator.SetBool(IsMovingHash, state == UnitState.Moving);
        }
    }
}