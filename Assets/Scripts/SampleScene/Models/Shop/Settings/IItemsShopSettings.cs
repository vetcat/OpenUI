using System.Collections.Generic;

namespace SampleScene.Models.Shop.Settings
{
    public interface IItemsShopSettings
    {
        List<ItemShopVo> Items { get; }
    }
}