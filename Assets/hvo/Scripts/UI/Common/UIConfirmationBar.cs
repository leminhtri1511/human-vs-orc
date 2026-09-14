using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HVO.Scripts.UI.Common
{
    public class UIConfirmationBar : MonoBehaviour
    {
        public UnityAction OnConfirm;
        public UnityAction OnCancel;

        [Header("Buttons")]
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        private void OnEnable()
        {
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        private void OnDisable()
        {
            _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
            _cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
        }

        private void OnConfirmButtonClicked()
        {
            OnConfirm?.Invoke();
        }

        private void OnCancelButtonClicked()
        {
            OnCancel?.Invoke();
        }
    }
}