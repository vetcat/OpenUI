using UnityEngine;

namespace Libs.OpenUI
{
    public class UiView : MonoBehaviour
    {
        public void Show(bool isShow = true)
        {
            gameObject.SetActive(isShow);
        }

        public void Hide()
        {
            if (IsShow())
                gameObject.SetActive(false);
        }

        public bool IsShow()
        {
            return gameObject.activeSelf;
        }
    }
}