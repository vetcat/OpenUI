using System;
using UnityEngine;

namespace Libs.OpenUI.ModalWindows
{
    public interface IModalWindowController
    {
        UiView InfoOkCancel(string caption, string description, Action handlerOk, Action handlerCancel);
        UiView InfoOk(string caption, string description, Action handlerClose = null);
        void ShowWait(bool show, string caption = "");

        //void ShowWait(bool show, string caption = "");
        void DestroyWindow(GameObject window);
    }
}