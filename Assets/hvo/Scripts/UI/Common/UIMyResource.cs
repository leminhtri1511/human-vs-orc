using System;
using System.Linq;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.Common;
using HVO.Scripts.ScriptableObjects.Events;
using TMPro;
using UnityEngine;

namespace HVO.Scripts.UI.Common
{
    public class UIMyResource : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private MyWalletSO _myWalletSO;
        [SerializeField] private GameResourcesType _resourceType;
        [SerializeField] private OnWalletUpdateEvent _onWalletUpdateEvent;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _amountText;

        private void OnEnable()
        {
            OnWalletUpdate();
            _onWalletUpdateEvent.EventRaised += OnWalletUpdate;
        }

        private void OnDisable()
        {
            _onWalletUpdateEvent.EventRaised -= OnWalletUpdate;
        }

        private void InitializeAmountText()
        {
            var myResource =
                _myWalletSO.MyResources.FirstOrDefault(resource => resource.ResourceSO.ResourceType == _resourceType);

            _amountText.text = FormatNumber.Format((double)myResource.Amount);
        }

        private void OnWalletUpdate()
        {
            InitializeAmountText();
        }
    }
}