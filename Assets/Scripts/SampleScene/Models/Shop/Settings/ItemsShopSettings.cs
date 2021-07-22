using System.Collections.Generic;
using Libs.OpenUI.Utils;
using UnityEngine;

namespace SampleScene.Models.Shop.Settings
{
    [CreateAssetMenu(menuName = "Settings/ItemsShopSettings", fileName = "ItemsShopSettings")]
    public class ItemsShopSettings : ScriptableObject, IItemsShopSettings
    {
        [KeyValue("Type")] [SerializeField] private List<ItemShopVo> _items;
        public List<ItemShopVo> Items => _items;
        
        private Dictionary<EItemShopType, ItemShopVo> _itemsCache;
        
        public List<ItemShopVo> GetItemsByGroup(EItemShopGroup group)
        {
            return _items.FindAll(f => f.Group == group);
        }
        
        public ItemShopVo GetItemByType(EItemShopType type)
        {
            return GetItemsCache()[type];
        }
        
        private Dictionary<EItemShopType, ItemShopVo> GetItemsCache()
        {
            if (_itemsCache == null)
            {
                _itemsCache = new Dictionary<EItemShopType, ItemShopVo>(_items.Count);
                foreach (var item in _items)
                {
                    _itemsCache.Add(item.Type, item);
                }
            }

            return _itemsCache;
        }
    }
}