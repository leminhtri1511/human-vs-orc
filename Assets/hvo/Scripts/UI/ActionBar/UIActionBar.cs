using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.ScriptableObjects.Events;
using HVO.Scripts.UI.Pool;
using HVO.Scripts.Units;
using UnityEngine;
using UnityEngine.Events;

namespace HVO.Scripts.UI.ActionBar
{
    public class UIActionBar : MonoBehaviour
    {
        public UnityAction<BuildActionSO> OnActionClick;
        
        [Header("Events")]
        [SerializeField] private OnUnitActionEvent _onUnitActionEvent;

        [Header("UI")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Pool")]
        [SerializeField] private UIActionButtonPool _actionButtonPool;

        private readonly List<UIActionButton> _cachedPool = new();

        public async UniTask SetupActionButtons(Unit activeUnit)
        {
            await ReleasePool();

            foreach (var action in activeUnit.ActionSOList)
            {
                var item = _actionButtonPool.Get(_rectTransform);

                item.Initialize(action);
                item.OnActionButtonClicked += OnUnitAction;

                _cachedPool.Add(item);
            }
        }

        public async UniTask ReleasePool()
        {
            foreach (var uiActionButton in _cachedPool)
            {
                uiActionButton.OnActionButtonClicked -= OnUnitAction;
                _actionButtonPool.Release(uiActionButton);
            }

            _cachedPool.Clear();
        }

        private void OnUnitAction(ActionSO action)
        {
            _onUnitActionEvent.RaiseEvent(action as BuildActionSO);
            OnActionClick?.Invoke(action as BuildActionSO);
        }
    }
}