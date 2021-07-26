using System;
using System.Collections.Generic;
using Libs.OpenUI.Utils;
using SampleScene.Models.Shop;
using UnityEngine;

namespace ProjectContext.Settings
{
    [Serializable]
    public class ItemShopGroupSprite
    {
        public EItemShopGroup Group;
        public Sprite Sprite;
    }
    
    [CreateAssetMenu(menuName = "Settings/IconSettings", fileName = "IconSettings")]
    public class IconSettings : ScriptableObject
    {
        public Sprite Clock;
        public Sprite Character;
        
        [KeyValue("Group")]
        public List<ItemShopGroupSprite> ItemShopGroupSprites;

        public Sprite GetItemShopGroupIcon(EItemShopGroup group)
        {
            var sprite = ItemShopGroupSprites.Find(f => f.Group == group);
            if (sprite != null)
                return sprite.Sprite;

            Debug.LogError($"IconSettings error item shop group sprite not found {group}");
            return null;
        }
    }
}