using Libs.OpenUI;
using Libs.OpenUI.Localization;
using UnityEngine.UI;
using Zenject;

namespace ProjectContext.UiViews
{
    public class ModalInfoOkView : UiViewCanvasGroup
    {
        public Text TextCaption;
        public Text TextDescription;
        public Button ButtonOk;
        public Button ButtonClose;
        public Button ButtonCloseBg;
        [Localization("Ok")]
        public Text TextOk;

        public class Factory : PlaceholderFactory<ModalInfoOkView>
        {
        }
    }
}
