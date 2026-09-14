using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.UI.Pool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HVO.Scripts.UI.ConfirmationBuildBar
{
    public class UIConfirmationBar : MonoBehaviour
    {
        public UnityAction OnConfirm;
        public UnityAction OnCancel;

        [Header("Buttons")]
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        [Header("Rect Transform")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Pool")]
        [SerializeField] private UIRequiredResourcePool _requiredResourcePool;

        private readonly List<UIRequiredResource> _cachePool = new();

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

        public async UniTask SetupRequiredResource(BuildActionSO buildActionSO)
        {
            await _requiredResourcePool.ClearPool(_cachePool);

            foreach (var requiredResource in buildActionSO.RequiredResources)
            {
                var uiRequiredResource = _requiredResourcePool.Get(_rectTransform);

                uiRequiredResource.Initialize(requiredResource);

                _cachePool.Add(uiRequiredResource);
            }
        }
    }
}