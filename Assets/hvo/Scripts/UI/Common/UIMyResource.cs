using System.Linq;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace HVO.Scripts.UI.Common
{
    public class UIMyResource : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private MyWalletSO _myWalletSO;
        [SerializeField] private GameResourcesType _resourceType;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _amountText;

        private void Start()
        {
            InitializeAmountText();
        }

        private void InitializeAmountText()
        {
            var myResource =
                _myWalletSO.MyResources.FirstOrDefault(resource => resource.ResourceSO.ResourceType == _resourceType);

            _amountText.text = FormatNumber.Format((double)myResource.Amount);
        }
    }
}