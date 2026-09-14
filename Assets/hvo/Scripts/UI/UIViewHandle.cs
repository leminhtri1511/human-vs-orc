using Cysharp.Threading.Tasks;
using HVO.Scripts.Managers;
using HVO.Scripts.UI.Common;
using HVO.Scripts.UI.Pool;
using UnityEngine;

namespace HVO.Scripts.UI
{
    public class UIViewHandle : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform _actionBarRT;
        [SerializeField] private UIActionBar _uiActionBar;

        [Header("Pool")]
        [SerializeField] private UIPointerPool _pointerPool;

        private void Start()
        {
            ToggleActionBarState(false);
        }

        public void InitializeUnitAction(GameManager gameManager)
        {
            if (!gameManager.ActiveUnit.HasActionSO)
            {
                ToggleActionBarState(false);
                return;
            }

            _uiActionBar.SetupActionButtons(gameManager).Forget();
        }

        public void ToggleActionBarState(bool isActive)
        {
            _actionBarRT.gameObject.SetActive(isActive);
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
    }
}