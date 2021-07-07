using Libs.OpenUI.ModalWindows;
using ProjectContext.UiViews;
using UnityEngine;
using Zenject;

namespace ProjectContext
{
    public class ModalUiInstaller : MonoInstaller
    {
        public Canvas CanvasModalUi;

        public ModalInfoOkCancelView ModalInfoOkCancelView;
        public ModalWaitView ModalWaitView;

        public override void InstallBindings()
        {
            Debug.Log("[UiModalWindowsInstaller] InstallBindings");
            
            Container.BindInterfacesAndSelfTo<ModalWindowController>().AsSingle().WithArguments(CanvasModalUi)
                .NonLazy();
            //bind modale window
            Container.BindFactory<ModalInfoOkCancelView, ModalInfoOkCancelView.Factory>()
                .FromComponentInNewPrefab(ModalInfoOkCancelView.gameObject);
            Container.BindFactory<ModalWaitView, ModalWaitView.Factory>()
                .FromComponentInNewPrefab(ModalWaitView.gameObject);
        }
    }
}