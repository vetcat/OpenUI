using System;
using Libs.OpenUI;
using Libs.OpenUI.Signals;
using Libs.OpenUI.UiEffects;
using SampleScene.UiViews.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace SampleScene.UiViews.Presenters
{
    public class UiDownLeftViewPresenter : UiPresenter<UiDownLeftView>
    {
        private readonly SignalBus _signalBus;
        private readonly IUiShopViewPresenter _uiShopViewPresenter;

        public UiDownLeftViewPresenter(SignalBus signalBus, IUiShopViewPresenter uiShopViewPresenter)
        {
            _signalBus = signalBus;
            _uiShopViewPresenter = uiShopViewPresenter;
        }

        public override void Initialize()
        {
            base.Initialize();
            View.EnableButtonEffects();

            //reaction to opening other Window from signal
            // _signalBus.GetStream<SignalShowView>().Where(data => data.View == _uiShopViewPresenter.GetUiView)
            //     .Subscribe(_ => UiShopViewShow())
            //     .AddTo(Disposables);

            //alternative reaction to opening other Window
            _uiShopViewPresenter.OnShow
                .Subscribe(_ => UiShopViewShow())
                .AddTo(Disposables);

            View.ButtonItemsShop
                .OnClickAsObservable()
                .Subscribe(_ => ButtonItemsShopClick())
                .AddTo(Disposables);
        }

        public override void ShowWithAnimation(Action complete = null)
        {
            Show();
            View.ExpandAnimation(View.Body, EAnimationTarget.Up, complete);
        }

        public override void HideWithAnimation(Action complete = null)
        {
            View.CollapseAnimation(View.Body, EAnimationTarget.Down, () =>
            {
                Hide();
                complete?.Invoke();
            });
        }

        private void UiShopViewShow()
        {
            Debug.LogError("UiShopViewShow");
            HideWithAnimation();
        }

        private void ButtonItemsShopClick()
        {
            _uiShopViewPresenter.Open();
        }
    }
}