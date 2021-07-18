using System.Collections.Generic;
using UnityEngine;

namespace SampleScene.Models.Shop.Settings
{
    [CreateAssetMenu(menuName = "Settings/ItemsShopSettings", fileName = "ItemsShopSettings")]
    public class ItemsShopSettings : ScriptableObject, IItemsShopSettings
    {
        [SerializeField] private List<ItemShopVo> _items;
        public List<ItemShopVo> Items => _items;
    }
}