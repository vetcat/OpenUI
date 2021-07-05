using UnityEngine;

namespace Libs.OpenUI.Signals
{
    public readonly struct SignalButtonClick
    {
        public readonly RectTransform RectTransform;
        public SignalButtonClick(RectTransform rectTransform)
        {
            RectTransform = rectTransform;
        }
    }
}