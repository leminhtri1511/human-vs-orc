using HVO.Scripts.Common;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HVO.Scripts.UI.Common
{
    public class UISpriteRendererLayout : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private OrderLayer _orderLayer = OrderLayer.Unknown;

        [Header("UI")]
        [SerializeField] private SpriteRenderer _defaultRenderer;
        [SerializeField] private TilemapRenderer _tilemapRenderer;

        private void Start()
        {
            if (_defaultRenderer != null) _defaultRenderer.sortingOrder = (int)_orderLayer;

            if (_tilemapRenderer != null) _tilemapRenderer.sortingOrder = (int)_orderLayer;
        }
    }
}