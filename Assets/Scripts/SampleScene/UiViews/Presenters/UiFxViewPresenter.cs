using Libs.OpenUI;
using SampleScene.UiViews.Views;
using SampleScene.UiViews.Views.UiFx;
using UnityEngine;

namespace SampleScene.UiViews.Presenters
{
    public class UiFxViewPresenter : UiPresenter<UiFxView>, IUiFxViewPresenter
    {
        private readonly RectTransform _coinsRectTransform;

        public UiFxViewPresenter(UiTopRightView uiTopRightView)
        {
            _coinsRectTransform = uiTopRightView.ImageIconCoins.GetComponent<RectTransform>();
        }

        public void ShowCollectSimpleAnimation(int amount, Sprite icon, Vector3 position)
        {
            var rectTarget = _coinsRectTransform;
            View.CollectSimpleAnimation(amount, icon, position, rectTarget);
        }

        public void ShowCollectAdvanceAnimation(int amount, Sprite icon, RectTransform rectSource)
        {
            var rectTarget = _coinsRectTransform;
            View.CollectAdvanceAnimation(amount, icon, rectSource, rectTarget);
        }
        
        public void ShowSpendingAnimation(int amount, Sprite icon, Vector3 position)
        {
            View.SpendingAnimation(amount, icon, position);
        }
    }
}