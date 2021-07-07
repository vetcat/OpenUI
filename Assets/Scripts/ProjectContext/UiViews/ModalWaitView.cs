using Libs.OpenUI;
using UnityEngine.UI;
using Zenject;

namespace ProjectContext.UiViews
{
    public class ModalWaitView : UiViewCanvasGroup
    {
        public Text TextCaption;
        public class Factory : PlaceholderFactory<ModalWaitView>
        {
        }
    }
}