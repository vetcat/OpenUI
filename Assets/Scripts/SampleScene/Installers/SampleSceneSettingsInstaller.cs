using SampleScene.Models.Shop.Settings;
using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    [CreateAssetMenu(fileName = "SampleSceneSettingsInstaller", menuName = "Installers/SampleSceneSettingsInstaller")]
    public class SampleSceneSettingsInstaller : ScriptableObjectInstaller<SampleSceneSettingsInstaller>
    {
        [SerializeField] private ItemsShopSettings _itemsShopSettings;

        public override void InstallBindings()
        {
            Debug.Log("[SampleSceneSettingsInstaller] InstallBindings");
            BindSettings();
        }

        private void BindSettings()
        {
            Container.Bind<IItemsShopSettings>().FromInstance(_itemsShopSettings);
        }
    }
}