using System.Collections.Generic;
using HVO.Scripts.Utils;
using UnityEngine;

namespace HVO.Scripts.ScriptableObjects
{
    public class MyWalletSO : ScriptableObject
    {
        [field: SerializeField] public List<ResourceInfo> MyResources { get; set; }
    }
}