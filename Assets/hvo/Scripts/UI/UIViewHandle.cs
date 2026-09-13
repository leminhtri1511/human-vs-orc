using Cysharp.Threading.Tasks;
using HVO.Scripts.Managers;
using HVO.Scripts.UI.Common;
using HVO.Scripts.UI.Pool;
using HVO.Scripts.Units;
using UnityEngine;
using UnityEngine.EventSystems;

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

        public bool IsPointerOverUIObject()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
            }
            else
            {
                return EventSystem.current.IsPointerOverGameObject();
            }
        }
    }
}