using Libs.OpenUI;
using Libs.OpenUI.Localization;
using UnityEngine.UI;
using Zenject;

namespace ProjectContext.UiViews
{
    public class ModalInfoOkCancelView : UiViewCanvasGroup
    {
        public Text TextCaption;
        public Text TextDescription;
        public Button ButtonOk;
        public Button ButtonCancel;
        [Localization("Cancel")]
        public Text TextCancel;
        [Localization("Ok")]
        public Text TextOk;

        public class Factory : PlaceholderFactory<ModalInfoOkCancelView>
        {
        }
    }
}
