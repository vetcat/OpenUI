using Libs.OpenUI;
using Libs.OpenUI.UiEffects;
using UnityEngine;

namespace SampleScene.UiViews.Presenters
{
    public interface IUiHintsViewPresenter
    {
        UiView OpenSimpleHint(string description, Vector3 position, EPivotTarget pivotTarget);
        void CloseHint(UiView hint);
    }
}