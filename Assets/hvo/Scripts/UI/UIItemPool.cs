using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace HVO.Scripts.UI
{
    public class UIItemPool<TItem> : MonoBehaviour where TItem : MonoBehaviour
    {
        [SerializeField] private TItem _prefab;

        private IObjectPool<TItem> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<TItem>(OnCreate, OnGet, OnRelease, OnDestroyItem);
        }

        private TItem OnCreate()
        {
            var uiItem = Instantiate(_prefab, transform);
            uiItem.gameObject.SetActive(false);
            return uiItem;
        }

        private void OnGet(TItem uiItem)
        {
            uiItem.gameObject.SetActive(true);
        }

        private void OnRelease(TItem item)
        {
            item.transform.SetParent(transform, false);
            item.gameObject.SetActive(false);
        }

        private void OnDestroyItem(TItem item)
        {
            if (item != null) Destroy(item.gameObject);
        }

        public TItem Get()
        {
            return _pool.Get();
        }

        public TItem Get(Transform newParent)
        {
            var item = _pool.Get();
            item.transform.SetParent(newParent, false);
            return item;
        }

        public void Release(TItem item)
        {
            if (item == null) return;

            _pool.Release(item);
        }

        public async UniTask ClearPool(List<TItem> items)
        {
            foreach (var item in items.ToList())
            {
                _pool.Release(item);
            }

            items.Clear();
        }

        public void Warmup(int count)
        {
            var cache = new List<TItem>(count);

            for (int i = 0; i < count; i++)
            {
                cache.Add(_pool.Get());
            }

            foreach (var item in cache)
            {
                _pool.Release(item);
            }
        }

        public float GetItemHeight()
        {
            return _prefab.GetComponent<RectTransform>().rect.height;
        }

        public float GetItemWidth()
        {
            return _prefab.GetComponent<RectTransform>().rect.width;
        }

        public void UpdateContentHeight(RectTransform transform, float spacing, int itemCount)
        {
            var height = GetItemHeight() * itemCount + spacing * (itemCount - 1);
            var width = transform.sizeDelta.x;
            transform.sizeDelta = new Vector2(width, height);
        }

        public void ClearAll()
        {
            _pool.Clear();
        }
    }
}