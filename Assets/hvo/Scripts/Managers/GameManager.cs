using HVO.Scripts.Units;
using UnityEngine;

namespace HVO.Scripts.Managers
{
    public class GameManager : SingletonManager<GameManager>
    {
        public Unit ActiveUnit;

        private Vector2 _initialTouchPosition;

        private void Update()
        {
            Vector2 inputPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;

            if (IsPressed())
            {
                _initialTouchPosition = inputPosition;
            }

            if (IsClicked())
            {
                if (Vector2.Distance(_initialTouchPosition, inputPosition) < 10)
                {
                    DetectClick(inputPosition);
                }
            }
        }

        private static bool IsClicked()
        {
            return Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
        }

        private static bool IsPressed()
        {
            return Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
        }

        public bool HasActiveUnit()
        {
            return ActiveUnit != null;
        }

        private void DetectClick(Vector2 inputPosition)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (HasClickedOnUnit(hit, out var unit))
            {
                HandleClickOnUnit(unit);
            }
            else
            {
                HandleClickOnGround(worldPoint);
            }
        }

        private bool HasClickedOnUnit(RaycastHit2D hit, out Unit unit)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<Unit>(out var clickedUnit))
            {
                unit = clickedUnit;
                return true;
            }

            unit = null;
            return false;
        }

        private void HandleClickOnUnit(Unit unit)
        {
            if (HasActiveUnit() && ActiveUnit == unit)
            {
                ActiveUnit.ToggleUnitSelectedState(false);
                ActiveUnit = null;
                return;
            }

            if (HasActiveUnit())
            {
                ActiveUnit.ToggleUnitSelectedState(false);
            }

            ActiveUnit = unit;
            ActiveUnit.ToggleUnitSelectedState(true);
        }

        private void HandleClickOnGround(Vector2 inputPosition)
        {
            if (!HasActiveUnit()) return;

            ActiveUnit.MoveTo(inputPosition);
        }
    }
}