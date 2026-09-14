using UnityEngine;
using UnityEngine.EventSystems;

namespace HVO.Scripts.Utils
{
    public static class HvoUtils
    {
        public static Vector2 InputPosition => Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;

        public static bool IsLeftClickOrTapDown =>
            Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

        public static bool IsLeftClickOrTapUp =>
            Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);

        public static Vector3 InputHoldWorldPosition
        {
            get
            {
                if (Camera.main != null)
                    return Input.touchCount > 0 ? Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) :
                        Input.GetMouseButton(0) ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Vector2.zero;
                return default;
            }
        }

        private static Vector2 _initialTouchPosition;

        public static bool TryGetShortClickPosition(out Vector2 inputPosition, float maxDistance = 5f)
        {
            inputPosition = InputPosition;

            if (IsLeftClickOrTapDown)
            {
                _initialTouchPosition = inputPosition;
            }

            if (!IsLeftClickOrTapUp) return false;

            return Vector2.Distance(_initialTouchPosition, inputPosition) < maxDistance;
        }

        public static bool TryGetHoldPosition(out Vector3 worldPosition)
        {
            if (Input.touchCount > 0)
            {
                if (Camera.main != null) worldPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                worldPosition = default;
                return true;
            }

            if (Input.GetMouseButton(0))
            {
                if (Camera.main != null) worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition = default;
                return true;
            }

            worldPosition = Vector3.zero;
            return false;
        }

        public static bool IsPointerOverUIElement()
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

        public static Vector3 SnapPlacementToGrid(Vector3 worldPosition)
        {
            return new Vector3(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y), 0);
        }
    }
}