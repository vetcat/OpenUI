using System;
using UnityEngine;

namespace SampleScene.Models.Shop
{
    [Serializable]
    public class ItemShopVo
    {
        public EItemShopType Type;
        public EItemShopGroup Group;
        public Sprite Icon;
        public int Amount;
    }
}