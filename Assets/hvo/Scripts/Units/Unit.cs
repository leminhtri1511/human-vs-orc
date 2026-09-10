using HVO.Scripts.AI;
using UnityEngine;

namespace HVO.Scripts.Units
{
    public abstract class Unit : MonoBehaviour
    {
        [Header("AI")]
        [SerializeField] private AIPawn _aiPawn;

        [Header("Unit States")]
        [SerializeField] private bool _isTargeted;
        [SerializeField] private bool _isMoving;

        [Header("UI Configs")]
        [SerializeField] private Material _highlightMaterial;
        [SerializeField] private Material _originalMaterial;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        protected bool IsTargeted => _isTargeted;

        protected bool IsMoving
        {
            get => _isMoving;
            set => _isMoving = value;
        }

        private void Awake()
        {
            _originalMaterial = _spriteRenderer.material;
            // m_HighlightMaterial = Resources.Load<Material>("Materials/Outline");
        }

        public void MoveTo(Vector3 destination)
        {
            var direction = (destination - transform.position).normalized;
            _spriteRenderer.flipX = direction.x < 0;

            _aiPawn.SetDestination(destination);
        }

        public void Select()
        {
            ToggleUnitSelectedState(true);
        }

        public void DeSelect()
        {
            ToggleUnitSelectedState(false);
        }

        private void ToggleUnitSelectedState(bool isSelected)
        {
            _spriteRenderer.material = isSelected ? _highlightMaterial : _originalMaterial;
            _isTargeted = isSelected;
        }
    }
}