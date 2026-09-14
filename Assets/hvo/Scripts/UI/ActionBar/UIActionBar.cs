using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HVO.Scripts.Managers;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.UI.Pool;
using UnityEngine;
using UnityEngine.Events;

namespace HVO.Scripts.UI.ActionBar
{
    public class UIActionBar : MonoBehaviour
    {
        public UnityAction OnActionClick;
        
        [Header("Events")]
        [SerializeField] private OnUnitActionEvent _onUnitActionEvent;

        [Header("UI")]
        [SerializeField] private RectTransform _rectTransform;

        [Header("Pool")]
        [SerializeField] private UIActionButtonPool _actionButtonPool;

        private readonly List<UIActionButton> _cachedPool = new();
        private GameManager _gameManager;

        public async UniTask SetupActionButtons(GameManager gameManager)
        {
            _gameManager = gameManager;
            await ReleasePool();

            foreach (var action in gameManager.ActiveUnit.ActionSOList)
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
            action.Execute(_gameManager);
            OnActionClick?.Invoke();
        }
    }
}