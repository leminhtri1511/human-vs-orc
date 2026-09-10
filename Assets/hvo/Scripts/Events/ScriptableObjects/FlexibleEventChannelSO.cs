using UnityEngine;
using UnityEngine.Events;

namespace HVO.Scripts.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "HVO/Events/Flexible Event Channel")]
    public class FlexibleEventChannelSO : ScriptableObject
    {
        public event UnityAction<object[]> EventRaised;

        public void RaiseEvent(params object[] args)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(args);
        }
    }
}