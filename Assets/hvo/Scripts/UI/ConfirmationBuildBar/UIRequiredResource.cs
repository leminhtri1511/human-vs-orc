using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.Services;
using HVO.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HVO.Scripts.UI.ConfirmationBuildBar
{
    public class UIRequiredResource : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private MyWalletSO _myWalletSO;

        [Header("UI")]
        [SerializeField] private Image _resourceIcon;
        [SerializeField] private TextMeshProUGUI _requiredAmountText;
        [SerializeField] private Color32 _validResourceColor = new(255, 255, 255, 255);
        [SerializeField] private Color32 _invalidResourceColor = new(255, 255, 255, 255);

        private ResourceService _resourceService;

        public void Initialize(ResourceInfo requiredResource)
        {
            _resourceIcon.sprite = requiredResource.ResourceSO.Sprite;
            _requiredAmountText.text = $"{requiredResource.Amount}";
            _requiredAmountText.color =
                HasEnoughResource(requiredResource) ? _validResourceColor : _invalidResourceColor;
        }

        private bool HasEnoughResource(ResourceInfo requiredResource)
        {
            _resourceService ??= new ResourceService(_myWalletSO);
            return _resourceService.HasEnoughSpecificResource(requiredResource);
        }
    }
}