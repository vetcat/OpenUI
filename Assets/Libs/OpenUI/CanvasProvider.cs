using Game.Providers;
using UnityEngine;

namespace Libs.OpenUI
{
    public class CanvasProvider : ICanvasProvider
    {
        public Canvas Canvas { get; }
        public float GetAspectFactor()
        {
            return (float)Screen.width / Screen.height;
        }

        public CanvasProvider(Canvas mainCanvas)
        {
            Canvas = mainCanvas;
        }
    }
}