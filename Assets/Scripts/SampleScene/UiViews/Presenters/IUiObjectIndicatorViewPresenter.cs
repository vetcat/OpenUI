using SampleScene.UiViews.Views.UiObjectIndicator;
using UnityEngine;

namespace SampleScene.UiViews.Presenters
{
    public interface IUiObjectIndicatorViewPresenter
    {
        UiIndicatorItemView AddCharacterIndicator(GameObject gameObject, Sprite icon);
        void RemoveCharacterIndicator(GameObject gameObject);
        void ShowCharacterIndicator(GameObject gameObject, bool show);
    }
}