using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HVO.Scripts.Managers;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.UI.Pool;
using UnityEngine;

namespace HVO.Scripts.UI.Common
{
    public class UIActionBar : MonoBehaviour
    {
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
            await _actionButtonPool.ClearPool(_cachedPool);

            foreach (var action in gameManager.ActiveUnit.ActionSOList)
            {
                var item = _actionButtonPool.Get(_rectTransform);

                item.Initialize(action);

                item.OnActionButtonClicked -= OnUnitAction;
                item.OnActionButtonClicked += OnUnitAction;

                _cachedPool.Add(item);
            }
        }

        private void OnUnitAction(ActionSO action)
        {
            action.Execute(_gameManager);
        }
    }
}