using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.UI;
using HVO.Scripts.Units;
using HVO.Scripts.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HVO.Scripts.Managers
{
    public enum OrderLayer
    {
        Unknown = -99,
        Water = -5,
        Rock = -4,
        Foam = -3,
        Elevations = -2,
        UnderTerrain = -1,
        Walkable = 0,
        Unit = 1,
        Pointer = 10,
        PendingPlacement = 20,
        AlwaysOnTop = 100
    }

    public class GameManager : SingletonManager<GameManager>
    {
        [Header("Events")]
        [SerializeField] private OnUnitActionEvent _onUnitActionEvent;

        [Header("Tilemaps")]
        [SerializeField] private Tilemap _walkableTilemap;
        [SerializeField] private Tilemap _overlayTilemap;

        [Header("Controllers")]
        [SerializeField] private UIViewHandle _uiViewHandle;

        public Unit ActiveUnit { get; private set; }
        private Vector2 _initialTouchPosition;
        private PlacementProcess _placementProcess;

        private void Update()
        {
            HandleTouchInput();
        }

        private void HandleTouchInput()
        {
            if (_placementProcess != null)
            {
                _placementProcess.Update();
            }
            else if (HvoUtils.TryGetShortClickPosition(out var inputPosition))
            {
                DetectClick(inputPosition);
            }
        }

        public void StartBuildProgress(BuildActionSO buildActionSO)
        {
            _placementProcess = new PlacementProcess(buildActionSO, _walkableTilemap, _overlayTilemap);

            _placementProcess.ShowPendingPlacement();
        }

        public bool HasActiveUnit()
        {
            return ActiveUnit != null;
        }

        public bool IsHumanoidUnit(Unit unit)
        {
            return unit is HumanoidUnit;
        }

        private void DetectClick(Vector2 inputPosition)
        {
            if (Camera.main == null || HvoUtils.IsPointerOverUIElement()) return;

            var worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
            var hit = Physics2D.Raycast(worldPoint, Vector2.zero);

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
            if (ActiveUnit == unit)
            {
                DeselectUnit();
                return;
            }

            SelectUnit(unit);
        }

        private void DeselectUnit()
        {
            ActiveUnit.ToggleUnitSelectedState(false);
            ActiveUnit = null;
            _uiViewHandle.ToggleActionBarState(false);
        }

        private void SelectUnit(Unit unit)
        {
            if (HasActiveUnit())
            {
                ActiveUnit.ToggleUnitSelectedState(false);
            }

            ActiveUnit = unit;
            ActiveUnit.ToggleUnitSelectedState(true);
            _uiViewHandle.ToggleActionBarState(true);
            _uiViewHandle.InitializeUnitAction(this);
        }

        private void HandleClickOnGround(Vector2 inputPosition)
        {
            if (!HasActiveUnit() || !IsHumanoidUnit(ActiveUnit)) return;

            _uiViewHandle.DisplayClickEffect(inputPosition);
            ActiveUnit.MoveTo(inputPosition);
        }

        public void Test()
        {
            Debug.Log("VAR");
        }
    }
}