using System;
using Libs.OpenUI;
using Libs.OpenUI.UiEffects;
using SampleScene.Models.Shop.Settings;
using SampleScene.UiViews.Views.UiItemsShop;
using UniRx;

namespace SampleScene.UiViews.Presenters
{
    public class UiShopViewPresenter : UiPresenter<UiShopView>, IUiShopViewPresenter
    {
        private readonly IItemsShopSettings _itemsShopSettings;

        public UiShopViewPresenter(IItemsShopSettings itemsShopSettings)
        {
            _itemsShopSettings = itemsShopSettings;
        }

        public override void Initialize()
        {
            base.Initialize();
            Hide();

            View.ButtonClose
                .OnClickAsObservable()
                .Subscribe(_ => HideWithAnimation())
                .AddTo(Disposables);
        }

        public override void ShowWithAnimation(Action complete = null)
        {
            Show();
            View.ExpandAnimation(View.Body, EAnimationTarget.Right, complete);
        }

        public override void HideWithAnimation(Action complete = null)
        {
            View.CollapseAnimation(View.Body, EAnimationTarget.Left, () =>
            {
                Hide();
                complete?.Invoke();
            });
        }

        public void Open()
        {
            ShowWithAnimation();
        }

        public void Close()
        {
            HideWithAnimation();
        }
    }
}