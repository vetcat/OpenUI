using System;
using System.Collections;
using Libs.OpenUI.UiEffects;
using ProjectContext.UiViews;
using UniRx;
using UnityEngine;
using Zenject;

namespace Libs.OpenUI.ModalWindows
{
    public class ModalWindowController : IModalWindowController
    {
        private readonly Canvas _canvas;
        readonly AsyncProcessor _asyncProcessor;
        private readonly SignalBus _signalBus;
        private readonly int _childCanvasDefaultCount;

        [Inject] private ModalInfoOkCancelView.Factory _factoryInfoOkCancel;
        [Inject] private ModalWaitView.Factory _factoryWait;

        private ModalWaitView _modalWaitWindow;

        public ModalWindowController(SignalBus signalBus, Canvas canvas, AsyncProcessor asyncProcessor)
        {
            _signalBus = signalBus;
            _canvas = canvas;
            _asyncProcessor = asyncProcessor;

            _childCanvasDefaultCount = _canvas.transform.childCount;
            if (_canvas.gameObject.activeSelf)
                _canvas.gameObject.SetActive(false);
        }

        #region window

        public void ShowWait(bool show, string caption = "")
        {
            if (show)
            {
                if (_modalWaitWindow != null)
                    return;

                _modalWaitWindow = _factoryWait.Create();
                _modalWaitWindow.TextCaption.text = caption;

                CreateWindow(_modalWaitWindow);
            }
            else
            {
                if (_modalWaitWindow == null)
                    return;

                DestroyWindow(_modalWaitWindow.gameObject);
                _modalWaitWindow = null;
            }
        }

        public UiView InfoOkCancel(string caption, string description, Action handlerOk, Action handlerCancel)
        {
            var window = _factoryInfoOkCancel.Create();
            CreateWindow(window);

            window.TextCaption.text = caption;
            window.TextDescription.text = description;

            window.ButtonOk.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    DestroyWindow(window.gameObject);
                    handlerOk?.Invoke();
                })
                .AddTo(window);

            window.ButtonCancel.OnClickAsObservable()
                .Subscribe(_ => Cancel())
                .AddTo(window);

            window.ButtonClose.OnClickAsObservable()
                .Subscribe(_ => Cancel())
                .AddTo(window);

            void Cancel()
            {
                DestroyWindow(window.gameObject);
                handlerCancel?.Invoke();
            }

            return window;
        }

        #endregion

        #region base

        private void CreateWindow(UiView window)
        {
            if (!_canvas.gameObject.activeSelf)
                _canvas.gameObject.SetActive(true);

            window.transform.SetParent(_canvas.transform);

            var rect = window.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = Vector2.zero;
            rect.localScale = Vector3.one;

            window.EnableButtonEffects();
            window.AddSignalClick(_signalBus);

            var modalView = (UiViewCanvasGroup) window;
            window.FadeIn(modalView.Body, modalView.CanvasGroup);
        }

        public void DestroyWindow(GameObject window)
        {
            _asyncProcessor.DestroyGameObject(window.gameObject);
            _asyncProcessor.StartCoroutine(CheckChild());
        }

        private IEnumerator CheckChild()
        {
            yield return new WaitForEndOfFrame();
            if (_canvas.transform.childCount == _childCanvasDefaultCount)
                _canvas.gameObject.SetActive(false);
        }

        #endregion
    }
}