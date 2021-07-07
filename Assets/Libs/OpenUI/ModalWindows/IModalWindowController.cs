using System;
using UnityEngine;

namespace Libs.OpenUI.ModalWindows
{
    public interface IModalWindowController
    {
        UiView InfoOkCancel(string caption, string description, Action handlerOk, Action handlerCancel);
        void ShowWait(bool show, string caption = "");

        //void ShowWait(bool show, string caption = "");
        void DestroyWindow(GameObject window);
    }
}