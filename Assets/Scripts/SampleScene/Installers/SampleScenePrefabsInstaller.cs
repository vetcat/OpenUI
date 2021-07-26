using SampleScene.Models.Character;
using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    [CreateAssetMenu(fileName = "SampleScenePrefabsInstaller", menuName = "Installers/SampleScenePrefabsInstaller")]
    public class SampleScenePrefabsInstaller : ScriptableObjectInstaller<SampleScenePrefabsInstaller>
    {
        [SerializeField] private CharacterView CharacterView; 
        public override void InstallBindings()
        {
            BindFactory();
        }

        private void BindFactory()
        {
            Container.BindFactory<CharacterView, CharacterView.Factory>()
                .FromComponentInNewPrefab(CharacterView.gameObject);
        }
    }
}