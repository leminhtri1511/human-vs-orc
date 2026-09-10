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
            CalculateVelocity();
        }

        private void CalculateVelocity()
        {
            Vector3 positionDelta = transform.position - LastPosition;

            Velocity = new Vector2(
                positionDelta.x,
                positionDelta.y
            ) / Time.deltaTime;

            LastPosition = transform.position;

            UnitStateChecking();
        }

        private void UnitStateChecking()
        {
            IsMoving = Velocity.magnitude > 0;
            UnitAnimator.SetBool(IsMovingHash, IsMoving);
        }
    }
}