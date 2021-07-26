using UnityEngine;

namespace SampleScene.UiViews.Presenters
{
    public interface IUiFxViewPresenter
    {
        void ShowCollectSimpleAnimation(int amount, Sprite icon, Vector3 position);
        void ShowCollectAdvanceAnimation(int amount, Sprite icon, RectTransform rectSource);
        void ShowSpendingAnimation(int amount, Sprite icon, Vector3 position);
    }
}