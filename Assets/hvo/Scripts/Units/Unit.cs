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
        [SerializeField] private Animator _unitAnimator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public bool IsTargeted => _isTargeted;

        public bool IsMoving
        {
            get => _isMoving;
            set => _isMoving = value;
        }

        protected Animator UnitAnimator => _unitAnimator;

        private void Awake()
        {
            _spriteRenderer.material = _originalMaterial;
            _isMoving = false;
            // m_HighlightMaterial = Resources.Load<Material>("Materials/Outline");
        }

        public void MoveTo(Vector3 destination)
        {
            var direction = (destination - transform.position).normalized;
            _spriteRenderer.flipX = direction.x < 0;

            _aiPawn.SetDestination(destination);
        }

        public void ToggleUnitSelectedState(bool isSelected)
        {
            _spriteRenderer.material = isSelected ? _highlightMaterial : _originalMaterial;
            _isTargeted = isSelected;
        }
    }
}