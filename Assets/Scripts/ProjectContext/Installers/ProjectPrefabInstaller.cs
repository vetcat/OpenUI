using Models.Player;
using ProjectContext.Settings;
using UnityEngine;
using Zenject;

namespace ProjectContext.Installers
{
    [CreateAssetMenu(fileName = "ProjectPrefabInstaller", menuName = "Installers/ProjectPrefabInstaller")]
    public class ProjectPrefabInstaller : ScriptableObjectInstaller<ProjectPrefabInstaller>
    {
        [SerializeField] private PlayerSetting _playerSetting;
        [SerializeField] private IconSettings _iconSettings;
        public override void InstallBindings()
        {
            Container.Bind<IPlayerSetting>().FromInstance(_playerSetting);
            Container.Bind<IconSettings>().FromInstance(_iconSettings);
        }
    }
}