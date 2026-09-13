using HVO.Scripts.UI.Common;
using HVO.Scripts.UI.Pool;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HVO.Scripts.UI
{
    public class UIViewHandle : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform _actionBarRT;
        [SerializeField] private UIPointToClickPool _pointToClickPool;

        private void Start()
        {
            ToggleActionBarState(false);
        }

        public void ToggleActionBarState(bool isActive)
        {
            _actionBarRT.gameObject.SetActive(isActive);
        }

        public void DisplayClickEffect(Vector2 worldPoint)
        {
            var point = _pointToClickPool.Get();

            point.transform.SetPositionAndRotation(worldPoint, Quaternion.identity);
            point.Play(ReleasePointToClick);
        }

        private void ReleasePointToClick(PointToClick point)
        {
            _pointToClickPool.Release(point);
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