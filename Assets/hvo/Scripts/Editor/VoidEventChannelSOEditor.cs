using HVO.Scripts.Events.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace HVO.Scripts.Editor
{
    [CustomEditor(typeof(VoidEventChannelSO), true)]
    public class VoidEventChannelSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            var eventSO = target as VoidEventChannelSO;
            if (eventSO != null && GUILayout.Button($"Raise {eventSO.name}"))
            {
                eventSO.RaiseEvent();
            }
        }
    }
}