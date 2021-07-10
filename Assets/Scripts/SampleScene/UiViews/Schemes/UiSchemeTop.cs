using Libs.OpenUI;
using SampleScene.UiViews.Presenters;

namespace SampleScene.UiViews.Schemes
{
    public class UiSchemeTop : UiSchemeBuilder
    {
        public override void Initialize()
        {
            AddPresenter<UiTopRightViewPresenter>();
            AddPresenter<UiTopLeftViewPresenter>();
        }
    }
}