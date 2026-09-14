using Cysharp.Threading.Tasks;
using HVO.Scripts.Managers;
using HVO.Scripts.UI.ActionBar;
using HVO.Scripts.UI.Common;
using HVO.Scripts.UI.ConfirmationBuildBar;
using HVO.Scripts.UI.Pool;
using UnityEngine;

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

        private GameManager _gameManager;

        private void Start()
        {
            ToggleActionBarState(false);
            ToggleConfirmationBarState(false);
        }

        private void OnEnable()
        {
            _uiActionBar.OnActionClick += ActionSelected;
            _uiConfirmationBar.OnConfirm += ConfirmBuildSelected;
            _uiConfirmationBar.OnCancel += CancelBuildSelected;
        }

        private void OnDisable()
        {
            _uiActionBar.OnActionClick -= ActionSelected;
            _uiConfirmationBar.OnConfirm -= ConfirmBuildSelected;
            _uiConfirmationBar.OnCancel -= CancelBuildSelected;
        }

        public void InitializeUnitAction(GameManager gameManager)
        {
            _gameManager = gameManager;
            if (!gameManager.ActiveUnit.HasActionSO)
            {
                ToggleActionBarState(false);
                return;
            }

            _uiActionBar.SetupActionButtons(gameManager).Forget();
        }

        public void ToggleActionBarState(bool isActive) => _actionBarRT.gameObject.SetActive(isActive);

        public void ToggleConfirmationBarState(bool isActive) => _confirmationBarRT.gameObject.SetActive(isActive);

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

        private void ActionSelected()
        {
            _uiActionBar.ReleasePool().Forget();
            ToggleConfirmationBarState(true);
        }

        private void CancelBuildSelected()
        {
            ToggleConfirmationBarState(false);
            _uiActionBar.SetupActionButtons(_gameManager).Forget();
        }

        public void ConfirmBuildSelected()
        {
        }
    }
}