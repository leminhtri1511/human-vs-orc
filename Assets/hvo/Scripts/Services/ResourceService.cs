using System.Collections.Generic;
using HVO.Scripts.ScriptableObjects;
using HVO.Scripts.Common;

namespace HVO.Scripts.Services
{
    public class ResourceService
    {
        private readonly MyWalletSO _myWallet;

        public ResourceService(MyWalletSO myWallet)
        {
            _myWallet = myWallet;
        }

        public bool HasEnoughAllResources(IReadOnlyList<ResourceInfo> requiredResources)
        {
            foreach (var requiredResource in requiredResources)
            {
                var currentAmount =
                    GetCurrentAmount(requiredResource.ResourceSO.ResourceType);

                if (currentAmount < requiredResource.Amount)
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasEnoughSpecificResource(ResourceInfo requiredResource)
        {
            var currentAmount = GetCurrentAmount(requiredResource.ResourceSO.ResourceType);

            return currentAmount >= requiredResource.Amount;
        }

        public bool TryConsume(IReadOnlyList<ResourceInfo> requiredResources)
        {
            if (!HasEnoughAllResources(requiredResources))
            {
                return false;
            }

            foreach (var requiredResource in requiredResources)
            {
                Remove(
                    requiredResource.ResourceSO.ResourceType,
                    requiredResource.Amount);
            }

            return true;
        }

        public void Add(GameResourcesType resourceType, int amount)
        {
            if (amount <= 0) return;

            var currentAmount = GetCurrentAmount(resourceType);

            SetAmount(resourceType, currentAmount + amount);
        }

        private int GetCurrentAmount(GameResourcesType resourceType)
        {
            foreach (var resource in _myWallet.MyResources)
            {
                if (resource.ResourceSO.ResourceType == resourceType)
                {
                    return resource.Amount;
                }
            }

            return 0;
        }

        private void Remove(GameResourcesType resourceType, int amount)
        {
            if (amount <= 0) return;

            var currentAmount = GetCurrentAmount(resourceType);

            SetAmount(resourceType, currentAmount - amount);
        }

        private void SetAmount(GameResourcesType resourceType, int amount)
        {
            for (var i = 0; i < _myWallet.MyResources.Count; i++)
            {
                var resource = _myWallet.MyResources[i];

                if (resource.ResourceSO.ResourceType != resourceType) continue;

                resource.Amount = amount;
                _myWallet.MyResources[i] = resource;

                return;
            }
        }
    }
}