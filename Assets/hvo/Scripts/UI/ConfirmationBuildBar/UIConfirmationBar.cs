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
        [Header("Data")]
        [SerializeField] private MyWalletSO _myWalletSO;

        [Header("Buttons")]
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        [Header("Rect Transform")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Pool")]
        [SerializeField] private UIRequiredResourcePool _requiredResourcePool;

        private readonly List<UIRequiredResource> _cachePool = new();

        private void OnDisable()
        {
            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        }

        public async UniTask SetupRequiredResources(BuildActionSO buildActionSO)
        {
            await _requiredResourcePool.ClearPool(_cachePool);

            foreach (var requiredResource in buildActionSO.RequiredResources)
            {
                var uiRequiredResource = _requiredResourcePool.Get(_rectTransform);

                uiRequiredResource.Initialize(requiredResource);

                _cachePool.Add(uiRequiredResource);
            }
        }

        public void ButtonHooks(UnityAction onConfirm, UnityAction onCancel)
        {
            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();

            _confirmButton.onClick.AddListener(onConfirm);
            _cancelButton.onClick.AddListener(onCancel);
        }
    }
}