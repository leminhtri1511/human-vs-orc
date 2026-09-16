using HVO.Scripts.Common;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.ScriptableObjects.Events;
using HVO.Scripts.Services;
using HVO.Scripts.UI;
using HVO.Scripts.Units;
using HVO.Scripts.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HVO.Scripts.Managers
{
    public class GameManager : SingletonManager<GameManager>
    {
        [Header("Events")]
        [SerializeField] private OnUnitActionEvent _onUnitActionEvent;
        [SerializeField] private OnWalletUpdateEvent _onWalletUpdateEvent;

        [Header("Data")]
        [SerializeField] private MyWalletSO _myWalletSO;

        [Header("Tilemaps")]
        [SerializeField] private Tilemap _walkableTilemap;
        [SerializeField] private Tilemap _overlayTilemap;
        [SerializeField] private Tilemap[] _unreachableTilemaps;

        [Header("Controllers")]
        [SerializeField] private UIViewHandle _uiViewHandle;

        public Unit ActiveUnit;
        private Vector2 _initialTouchPosition;
        private PlacementProcess _placementProcess;
        private BuildingProcess _buildingProcess;
        private BuildActionSO _currentBuildActionSO;
        private ResourceService _resourceService;

        protected override void Awake()
        {
            _resourceService = new ResourceService(_myWalletSO, _onWalletUpdateEvent);
        }

        private void Update()
        {
            HandleTouchInput();

            if (ActiveUnit is null) return;
            Debug.Log($"State: {ActiveUnit.CurrentState} | Task: {ActiveUnit.CurrentTask}");
        }

        private void OnEnable()
        {
            _onUnitActionEvent.EventRaised += StartPendingPlacement;
        }

        private void OnDisable()
        {
            _onUnitActionEvent.EventRaised -= StartPendingPlacement;
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

        private void StartPendingPlacement(BuildActionSO buildActionSO)
        {
            if (_placementProcess != null) return;

            _currentBuildActionSO = buildActionSO;

            _placementProcess =
                new PlacementProcess(buildActionSO, _walkableTilemap, _overlayTilemap, _unreachableTilemaps);
            _placementProcess.ShowPendingPlacement();

            _uiViewHandle.InitializeRequiredResource(buildActionSO);
            _uiViewHandle.OnButtonsCallback(StartBuildingProgress, CancelBuildPlacement);
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
            _uiViewHandle.InitializeUnitActions(ActiveUnit);
        }

        private void HandleClickOnGround(Vector2 inputPosition)
        {
            if (!HasActiveUnit() || !IsHumanoidUnit(ActiveUnit)) return;

            _uiViewHandle.DisplayClickEffect(inputPosition);
            ActiveUnit.MoveTo(inputPosition);
        }

        private void StartBuildingProgress()
        {
            if (!CanStartBuild(out var placementPosition)) return;

            _buildingProcess = new BuildingProcess(_currentBuildActionSO, placementPosition, ActiveUnit as WorkerUnit);


            ExecuteCallback();
        }

        private bool CanStartBuild(out Vector3 placementPosition)
        {
            if (!_resourceService.HasEnoughAllResources(_currentBuildActionSO.RequiredResources))
            {
                Debug.Log("Not Enough Resources");
                placementPosition = default;
                return false;
            }

            if (!_placementProcess.TryFinalizePlacement(out placementPosition))
            {
                Debug.Log("Placement Incorrect");
                return false;
            }

            if (!_resourceService.TryConsume(_currentBuildActionSO.RequiredResources)) return false;

            return true;
        }

        private void CancelBuildPlacement()
        {
            _placementProcess.ClearPendingPlacement();

            ExecuteCallback();
        }

        private void ExecuteCallback()
        {
            _uiViewHandle.HandleBeforeCallback();
            _placementProcess = null;
            _currentBuildActionSO = null;
        }
    }
}