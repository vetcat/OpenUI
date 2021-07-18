using SampleScene.Models.Shop.Settings;
using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    [CreateAssetMenu(fileName = "SampleScenePrefabInstaller", menuName = "Installers/SampleScenePrefabInstaller")]
    public class SampleScenePrefabInstaller : ScriptableObjectInstaller<SampleScenePrefabInstaller>
    {
        [SerializeField] private ItemsShopSettings _itemsShopSettings;

        public override void InstallBindings()
        {
            Container.Bind<IItemsShopSettings>().FromInstance(_itemsShopSettings);
        }
    }
}