using Libs.OpenUI;
using Libs.OpenUI.Utils;
using SampleScene.UiViews.Presenters;
using SampleScene.UiViews.Schemes;
using SampleScene.UiViews.Views;
using SampleScene.UiViews.Views.UiHints;
using SampleScene.UiViews.Views.UiItemsShop;
using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    [CreateAssetMenu(fileName = "SampleSceneUiInstaller", menuName = "Installers/SampleSceneUiInstaller")]
    public class SampleSceneUiInstaller : ScriptableObjectInstaller<SampleSceneUiInstaller>
    {
        public UiTopCenterView UiTopCenterView;
        public UiTopRightView UiTopRightView;
        public UiTopLeftView UiTopLeftView;
        public UiDownLeftView UiDownLeftView;
        public UiSettingsView UiSettingsView;
        public UiShopView UiShopView;
        public UiHintsView UiHintsView;

        public override void InstallBindings()
        {
            Debug.Log("[SampleSceneUiInstaller] InstallBindings");

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
            Container.BindViewPresenter<UiTopCenterView, UiTopCenterViewPresenter>(
                UiTopCenterView, canvas);
            Container.BindViewPresenter<UiTopRightView, UiTopRightViewPresenter>(
                UiTopRightView, canvas);
            Container.BindViewPresenter<UiTopLeftView, UiTopLeftViewPresenter>(
                UiTopLeftView, canvas);
            Container.BindViewPresenter<UiDownLeftView, UiDownLeftViewPresenter>(
                UiDownLeftView, canvas);
            Container.BindViewPresenter<UiSettingsView, UiSettingsViewPresenter>(
                UiSettingsView, canvas);
            Container.BindViewPresenter<UiShopView, UiShopViewPresenter>(
                UiShopView, canvas);
            Container.BindViewPresenter<UiHintsView, UiHintsViewPresenter>(
                UiHintsView, canvas);
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