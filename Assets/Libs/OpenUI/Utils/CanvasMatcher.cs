using UnityEngine;
using UnityEngine.UI;

namespace Libs.OpenUI.Utils
{
    public class CanvasMatcher : MonoBehaviour
    {
        private float MIN_SCREEN_FACTOR_MATCH = 1.777f; // 1280 x 720

        void Start()
        {
            var canvasScaler = GetComponent<CanvasScaler>();

            if (canvasScaler == null)
            {
                Debug.LogError("[CanvasMatcher] component CanvasScaler not found " + gameObject.name);
                return;
            }

            var scaleFactor = Screen.width / Screen.height;
            canvasScaler.matchWidthOrHeight = scaleFactor < MIN_SCREEN_FACTOR_MATCH ? 0 : 1;
        }
    }
}