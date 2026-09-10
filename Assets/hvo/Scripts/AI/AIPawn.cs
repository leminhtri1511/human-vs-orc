using UnityEngine;

namespace HVO.Scripts.AI
{
    public class AIPawn : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private float _movingSpeed = 5f;

        private Vector3 _destination;

        public Vector3 Destination => _destination;

        private void Update()
        {
            ApplyDestination();
        }

        public void SetDestination(Vector3 destination)
        {
            _destination = destination;
        }

        private void ApplyDestination()
        {
            if (_destination == Vector3.zero) return;

            var direction = _destination - transform.position;
            transform.position += _movingSpeed * Time.deltaTime * direction.normalized;

            var distanceToDestination = Vector3.Distance(transform.position, _destination);
            if (distanceToDestination < 0.1f)
            {
                _destination = Vector3.zero;
            }
        }
    }
}