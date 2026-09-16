using System.Collections.Generic;
using HVO.Scripts.AI;
using HVO.Scripts.Common;
using HVO.Scripts.ScriptableObjects;
using UnityEngine;

namespace HVO.Scripts.Units
{
    public abstract class Unit : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private List<ActionSO> _actionSOList;

        [Header("AI")]
        [SerializeField] private AIPawn _aiPawn;

        [Header("Unit States")]
        [SerializeField] private bool _isTargeted;
        [SerializeField] private bool _isMoving;

        [Header("UI Configs")]
        [SerializeField] private Material _highlightMaterial;
        [SerializeField] private Material _originalMaterial;
        [SerializeField] private Animator _unitAnimator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _objectDetectionRadius = 3f;

        public UnitState CurrentState { get; protected set; } = UnitState.Idle;
        public UnitTask CurrentTask { get; protected set; } = UnitTask.Unknown;
        public Unit Target { get; protected set; }
        public List<ActionSO> ActionSOList => _actionSOList;
        public bool HasActionSO => ActionSOList.Count > 0;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public float ObjectDetectionRadius => _objectDetectionRadius;
        public bool HasTarget => Target != null;
        public bool IsTargeted => _isTargeted;

        public bool IsMoving
        {
            get => _isMoving;
            set => _isMoving = value;
        }

        protected Animator UnitAnimator => _unitAnimator;

        private void Awake()
        {
            OnAwake();
        }

        private void OnAwake()
        {
            _spriteRenderer.material = _originalMaterial;
            _isMoving = false;
            _isTargeted = false;
        }

        public void SetTask(UnitTask task)
        {
            OnSetTask(CurrentTask, task);
        }

        public void SetState(UnitState state)
        {
            OnSetState(CurrentState, state);
        }

        public void SetTarget(Unit target)
        {
            Target = target;
        }

        public void MoveTo(Vector3 destination)
        {
            var direction = (destination - transform.position).normalized;
            _spriteRenderer.flipX = direction.x < 0;

            _aiPawn?.SetDestination(destination);
            OnSetDestination();
        }

        public void ToggleUnitSelectedState(bool isSelected)
        {
            _spriteRenderer.material = isSelected ? _highlightMaterial : _originalMaterial;
            _isTargeted = isSelected;
        }

        protected virtual void OnSetDestination()
        {
        }

        protected virtual void OnSetTask(UnitTask oldTask, UnitTask newTask)
        {
            CurrentTask = newTask;
        }

        protected virtual void OnSetState(UnitState oldState, UnitState newState)
        {
            CurrentState = newState;
        }

        protected Collider2D[] RunProximityObjectDetection()
        {
            return Physics2D.OverlapCircleAll(transform.position, _objectDetectionRadius);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0, 1, 0.3f);
            Gizmos.DrawSphere(transform.position, _objectDetectionRadius);
        }
    }
}