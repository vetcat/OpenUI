using Libs.OpenUI;
using SampleScene.UiViews.Schemes;
using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    [CreateAssetMenu(fileName = "SampleSceneUiPrefabsInstaller", menuName = "Installers/SampleSceneUiPrefabsInstaller")]
    public class SampleSceneUiPrefabsInstaller : ScriptableObjectInstaller<SampleSceneUiPrefabsInstaller>
    {
        public override void InstallBindings()
        {
            Debug.Log("[SampleSceneUiPrefabsInstaller] InstallBindings");
            
            BindCanvas();
            BindCanvasFx();
            BindCanvasDynamic();
        }
        
        private void BindCanvas()
        {
            var canvas = GetCanvas("CanvasUi");
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
            var canvas = GetCanvas("CanvasFx");
            if (canvas)
                BindViewsFx(canvas);
        }

        private void BindCanvasDynamic()
        {
            var canvas = GetCanvas("CanvasDynamic");
            if (canvas)
                BindDynamicUi(canvas);
        }

        private void BindViews(Canvas canvas)
        {
            // Container.BindViewController<UiBackgroundCircleView, UiBackgroundCircleViewController>(
            //     UiBackgroundCircleView, canvas);
        }
        
        private void BindViewsFx(Canvas canvas)
        {
            //Container.BindViewController<UiFxView, UiFxViewController>(UiFxView, canvas);
        }

        private void BindDynamicUi(Canvas canvas)
        {
            // Container.BindViewController<UiBuildingIndicatorView, UiBuildingIndicatorViewController>(
            //     UiBuildingIndicatorView, canvas);
        }

        private void BindSchemes()
        {
            Container.BindInterfacesAndSelfTo<UiSchemeTop>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UiSchemeDown>().AsSingle().NonLazy();
        }

        private Canvas GetCanvas(string tag)
        {
            var canvasGo = GameObject.FindWithTag(tag);
            if (canvasGo == null)
            {
                Debug.LogError($"Canvas {tag} not found");
                return null;
            }

            return canvasGo.GetComponent<Canvas>();
        }
    }
}