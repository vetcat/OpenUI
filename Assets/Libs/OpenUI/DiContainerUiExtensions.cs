using UnityEngine;
using Zenject;

namespace Libs.OpenUI
{
    public static class DiContainerUiExtensions
    {
        public static void BindViewPresenter<TView, TPresenter>(this DiContainer container, UiView view, Canvas parent) where TView : UiView where TPresenter : UiPresenter
        {
            container.BindInterfacesAndSelfTo<TView>().FromComponentInNewPrefab(view).UnderTransform(parent.transform).AsSingle();
            container.BindPresenter<TPresenter>();
        }
        
        public static void BindViewPresenter<TView, TPresenter>(this DiContainer container, UiView view, GameObject parent) where TView : UiView where TPresenter : UiPresenter
        {
            container.BindInterfacesAndSelfTo<TView>().FromComponentInNewPrefab(view).UnderTransform(parent.transform).AsSingle();
            container.BindPresenter<TPresenter>();
        }
        
        public static void BindView<TView>(this DiContainer container, UiView view, Canvas parent) where TView : UiView
        {
            container.BindInterfacesAndSelfTo<TView>().FromComponentInNewPrefab(view).UnderTransform(parent.transform).AsSingle();
        }

        public static void BindViewPresenterWithArgument<TView, TPresenter, T>(this DiContainer container, GameObject viewPrefab, Transform parent, T paramArgument) where TView : UiView where TPresenter : UiPresenter
        {
            container.BindInterfacesAndSelfTo<TView>().FromComponentInNewPrefab(viewPrefab).UnderTransform(parent).AsSingle();
            container.BindPresenterWithArgument<TPresenter, T>(paramArgument);
        }

        private static void BindPresenter<TPresenter>(this DiContainer container) where TPresenter : UiPresenter
        {
            container.BindInterfacesAndSelfTo<TPresenter>().AsSingle().NonLazy();
        }

        private static void BindPresenterWithArgument<TPresenter, T>(this DiContainer container, T paramArgument) where TPresenter : UiPresenter
        {
            container.BindInterfacesAndSelfTo<TPresenter>().AsSingle().WithArguments(paramArgument).NonLazy();
        }
    }
}
