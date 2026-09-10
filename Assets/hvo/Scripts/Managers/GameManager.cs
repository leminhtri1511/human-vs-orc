using UnityEngine;

namespace HVO.Scripts.Managers
{
    public class GameManager : SingletonManager<GameManager>
    {
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

        private static void DetectClick(Vector2 inputPosition)
        {
            Debug.Log(inputPosition);
        }
    }
}