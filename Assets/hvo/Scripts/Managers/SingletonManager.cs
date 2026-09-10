using UnityEngine;

namespace HVO.Scripts.Managers
{
    public abstract class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected virtual void Awake()
        {
            var managers = FindObjectsByType<T>(FindObjectsSortMode.None);

            if (managers.Length <= 1) return;
            Destroy(gameObject);
        }

        public static T Get()
        {
            var tag = typeof(T).Name;
            var managerObject = GameObject.FindWithTag(tag);

            if (managerObject != null)
            {
                return managerObject.GetComponent<T>();
            }

            GameObject go = new(tag)
            {
                tag = tag
            };

            return go.AddComponent<T>();
        }
    }
}