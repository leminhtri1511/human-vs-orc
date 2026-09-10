using UnityEngine;

namespace HVO.Scripts.Units
{
    public class HumanoidUnit : Unit
    {
        protected Vector2 Velocity;
        protected Vector3 LastPosition;

        private void Update()
        {
            CalculateVelocity();
        }

        private void CalculateVelocity()
        {
            Velocity = new Vector2(
                (transform.position.x - LastPosition.x),
                (transform.position.y - LastPosition.y)
            ) / Time.deltaTime;

            LastPosition = transform.position;

            UnitStateChecking();
        }

        private void UnitStateChecking()
        {
            IsMoving = Velocity.magnitude > 0;
        }
    }
}