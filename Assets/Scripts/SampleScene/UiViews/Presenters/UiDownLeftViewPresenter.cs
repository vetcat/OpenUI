using System;
using Libs.OpenUI;
using Libs.OpenUI.Signals;
using Libs.OpenUI.UiEffects;
using SampleScene.UiViews.Views;
using UniRx;
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

            //reaction to show and hide other Window from signal (can be used for example for analytics)
            // _signalBus.GetStream<SignalShowView>().Where(data => data.View == _uiShopViewPresenter.GetUiView)
            //     .Subscribe(_ => UiShopViewShow())
            //     .AddTo(Disposables);
            //
            // _signalBus.GetStream<SignalHideView>().Where(data => data.View == _uiShopViewPresenter.GetUiView)
            //     .Subscribe(_ =>  UiShopViewHide())
            //     .AddTo(Disposables);

            //alternative reaction to show and hide other Window
            _uiShopViewPresenter.OnShow
                .Subscribe(_ => UiShopViewShow())
                .AddTo(Disposables);

            _uiShopViewPresenter.OnHide
                .Subscribe(_ => UiShopViewHide())
                .AddTo(Disposables);

            View.ButtonItemsShop
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    if (View.ButtonItemsShop.enabled)
                        ButtonItemsShopClick();
                })
                .AddTo(Disposables);
        }

        public override void ShowWithAnimation(Action complete = null)
        {
            Show();
            View.ExpandAnimation(View.Body, EAnimationTarget.Up, () =>
            {
                View.ButtonItemsShop.enabled = true;
                complete?.Invoke();
            });
        }

        public override void HideWithAnimation(Action complete = null)
        {
            View.ButtonItemsShop.enabled = false;
            View.CollapseAnimation(View.Body, EAnimationTarget.Down, () =>
            {
                Hide();
                complete?.Invoke();
            });
        }

        private void UiShopViewShow()
        {
            HideWithAnimation();
        }

        private void UiShopViewHide()
        {
            ShowWithAnimation();
        }

        private void ButtonItemsShopClick()
        {
            _uiShopViewPresenter.Open();
        }
    }
}