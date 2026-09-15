using Cysharp.Threading.Tasks;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.UI.ActionBar;
using HVO.Scripts.UI.Common;
using HVO.Scripts.UI.ConfirmationBuildBar;
using HVO.Scripts.UI.Pool;
using HVO.Scripts.Units;
using UnityEngine;
using UnityEngine.Events;

namespace HVO.Scripts.UI
{
    public class UIViewHandle : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private UIActionBar _uiActionBar;
        [SerializeField] private UIConfirmationBar _uiConfirmationBar;

        [Header("Rect Transform")]
        [SerializeField] private RectTransform _actionBarRT;
        [SerializeField] private RectTransform _confirmationBarRT;

        [Header("Pool")]
        [SerializeField] private UIPointerPool _pointerPool;

        private Unit _activeUnit;
        private UnityAction _confirmBuild;
        private UnityAction _cancelBuild;

        private void Start()
        {
            ToggleConfirmationBarState(false);
            ToggleActionBarState(false);
        }

        public void ToggleActionBarState(bool isActive) => _actionBarRT.gameObject.SetActive(isActive);
        public void ToggleConfirmationBarState(bool isActive) => _confirmationBarRT.gameObject.SetActive(isActive);

        public void InitializeUnitActions(Unit activeUnit)
        {
            _activeUnit = activeUnit;

            if (!activeUnit.HasActionSO)
            {
                ToggleActionBarState(false);
                return;
            }

            _uiActionBar.SetupActionButtons(activeUnit).Forget();
        }

        public void InitializeRequiredResource(BuildActionSO buildActionSO)
        {
            _uiActionBar.ReleasePool().Forget();
            ToggleConfirmationBarState(true);

            _uiConfirmationBar.SetupRequiredResources(buildActionSO).Forget();
        }

        public void DisplayClickEffect(Vector2 worldPoint)
        {
            var point = _pointerPool.Get();

            point.transform.SetPositionAndRotation(worldPoint, Quaternion.identity);
            point.Play(ReleasePointToClick);
        }

        private void ReleasePointToClick(UIPointer point)
        {
            _pointerPool.Release(point);
        }

        public void OnButtonsCallback(UnityAction onConfirm, UnityAction onCancel)
        {
            _uiConfirmationBar.ButtonHooks(onConfirm, onCancel);
        }

        public void HandleBeforeCallback()
        {
            ToggleConfirmationBarState(false);
            _uiActionBar.SetupActionButtons(_activeUnit).Forget();
        }
    }
}