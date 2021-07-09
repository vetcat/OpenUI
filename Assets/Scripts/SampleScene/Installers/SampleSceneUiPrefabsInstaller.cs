using Libs.OpenUI;
using Libs.OpenUI.Utils;
using SampleScene.UiViews.Presenters;
using SampleScene.UiViews.Schemes;
using SampleScene.UiViews.Views;
using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    [CreateAssetMenu(fileName = "SampleSceneUiPrefabsInstaller", menuName = "Installers/SampleSceneUiPrefabsInstaller")]
    public class SampleSceneUiPrefabsInstaller : ScriptableObjectInstaller<SampleSceneUiPrefabsInstaller>
    {
        public UiTopRightView UiTopRightView;
        public UiSettingsView UiSettingsView;
        
        public override void InstallBindings()
        {
            Debug.Log("[SampleSceneUiPrefabsInstaller] InstallBindings");
            
            BindCanvas();
            BindCanvasFx();
            BindCanvasDynamic();
        }
        
        private void BindCanvas()
        {
            var canvas = ExtensionsUi.GetCanvas("CanvasUi");
            if (canvas)
            {
                BindViews(canvas);
                BindSchemes();
                Container.BindInterfacesAndSelfTo<CanvasProvider>().AsSingle().WithArguments(canvas).NonLazy();
                Container.BindInterfacesAndSelfTo<WindowsController>().AsSingle().NonLazy();
            }
        }

        private void BindCanvasFx()
        {
            var canvas = ExtensionsUi.GetCanvas("CanvasFx");
            if (canvas)
                BindViewsFx(canvas);
        }

        private void BindCanvasDynamic()
        {
            var canvas = ExtensionsUi.GetCanvas("CanvasDynamic");
            if (canvas)
                BindDynamicUi(canvas);
        }

        private void BindViews(Canvas canvas)
        {
            Container.BindViewPresenter<UiTopRightView, UiTopRightViewPresenter>(
                UiTopRightView, canvas);
            Container.BindViewPresenter<UiSettingsView, UiSettingsViewPresenter>(
                UiSettingsView, canvas);
        }
        
        private void BindViewsFx(Canvas canvas)
        {
        }

        private void BindDynamicUi(Canvas canvas)
        {
        }

        private void BindSchemes()
        {
            Container.BindInterfacesAndSelfTo<UiSchemeTop>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UiSchemeDown>().AsSingle().NonLazy();
        }
    }
}