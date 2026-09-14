using HVO.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HVO.Scripts.UI.ConfirmationBuildBar
{
    public class UIRequiredResource : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image _resourceIcon;
        [SerializeField] private TextMeshProUGUI _requiredAmountText;

        public void Initialize(RequiredResourceConfig requiredResource)
        {
            _resourceIcon.sprite = requiredResource.ResourceSO.Sprite;
            _requiredAmountText.text = $"{requiredResource.RequiredAmount}";
        }
    }
}