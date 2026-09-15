using HVO.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HVO.Scripts.UI.ActionBar
{
    public class UIActionButton : MonoBehaviour
    {
        public UnityAction<ActionSO> OnActionButtonClicked;

        [Header("UI")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _button;

        private ActionSO _actionSO;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnActionClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnActionClick);
        }

        public void Initialize(ActionSO actionSO)
        {
            _actionSO = actionSO;
            _iconImage.sprite = actionSO.ActionIcon;
        }

        private void OnActionClick()
        {
            OnActionButtonClicked?.Invoke(_actionSO);
        }
    }
}