using Libs.OpenUI.ModalWindows;
using ProjectContext.UiViews;
using UnityEngine;
using Zenject;

namespace ProjectContext.Installers
{
    public class ModalUiInstaller : MonoInstaller
    {
        public Canvas CanvasModalUi;

        public ModalInfoOkCancelView ModalInfoOkCancelView;
        public ModalWaitView ModalWaitView;


        public override void InstallBindings()
        {
            Debug.Log("[ModalUiInstaller] InstallBindings");

            BindCanvas(CanvasModalUi);
        }
        
        private void BindCanvas(Canvas canvas)
        {
            Container.BindInterfacesAndSelfTo<ModalWindowController>().AsSingle().WithArguments(canvas)
                .NonLazy();
            //bind modale window
            Container.BindFactory<ModalInfoOkCancelView, ModalInfoOkCancelView.Factory>()
                .FromComponentInNewPrefab(ModalInfoOkCancelView.gameObject);
            // Container.BindFactory<ModalWaitView, ModalWaitView.Factory>()
            //     .FromComponentInNewPrefab(ModalWaitView.gameObject);
        }
    }
}