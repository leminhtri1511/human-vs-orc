using UnityEngine;
using UnityEngine.Events;

namespace HVO.Scripts.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "HVO/Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public UnityAction EventRaised;

        public void RaiseEvent()
        {
            OnRaiseEvent();
        }

        private void OnRaiseEvent()
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Events was raised on {name} but no one was listening.");
                return;
            }

            EventRaised?.Invoke();
        }
    }
}