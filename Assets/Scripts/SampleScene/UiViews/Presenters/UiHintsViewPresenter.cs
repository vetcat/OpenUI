using System;
using Game.Providers;
using Libs.OpenUI;
using Libs.OpenUI.Signals;
using Libs.OpenUI.UiEffects;
using SampleScene.UiViews.Views.UiHints;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SampleScene.UiViews.Presenters
{
    public class UiHintsViewPresenter : UiPresenter<UiHintsView>, IUiHintsViewPresenter
    {
        private readonly SignalBus _signalBus;
        private readonly ICanvasProvider _canvasProvider;
        private CompositeDisposable _hideDisposable = new CompositeDisposable();
        private IDisposable _closeDisposable;
        private IDisposable _hintDisposable;
        private UiView _hintLocal;

        public UiHintsViewPresenter(SignalBus signalBus, ICanvasProvider canvasProvider)
        {
            _signalBus = signalBus;
            _canvasProvider = canvasProvider;
        }

        public override void Initialize()
        {
            base.Initialize();

            View.EnableButtonEffects();
            View.transform.SetAsLastSibling();
            View.ImageHintBackground.gameObject.SetActive(false);
            Hide();

            AddSignalClick();
            DeactivateAll();

            _signalBus.GetStream<SignalShowView>()
                .Subscribe(data =>
                {
                    if (data.View != View)
                        HideAndDispose();
                })
                .AddTo(Disposables);
        }

        public void CloseHint(UiView hint)
        {
            hint.gameObject.SetActive(false);
            Hide();
        }

        private void HideAndDispose(bool useDelay = true)
        {
            _hideDisposable?.Dispose();
            _hideDisposable = new CompositeDisposable();
            _closeDisposable?.Dispose();

            if (useDelay)
            {
                Observable.NextFrame()
                    .Subscribe(_ => { HideAll(); });
            }
            else
            {
                HideAll();
            }
        }

        private void AddSignalClick()
        {
            for (var i = 0; i < View.Root.childCount; i++)
            {
                var child = View.Root.GetChild(i);
                var uiView = child.GetComponent<UiView>();
                if (uiView != null)
                {
                    uiView.AddSignalClick(_signalBus);
                }
            }
        }

        private void DeactivateAll()
        {
            for (var i = 0; i < View.Root.childCount; i++)
            {
                View.Root.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void HideAll()
        {
            View.ImageHintBackground.gameObject.SetActive(false);
            DeactivateAll();
            base.Hide();
        }

        public UiView OpenSimpleHint(string description, Vector3 position, EPivotTarget pivotTarget)
        {
            Show();
            var hintView = View.HintSimpleView;
            hintView.gameObject.SetActive(true);

            var hintPosition = position + GetHintOffsetWithPointer(pivotTarget, hintView.Pointer);

            hintView.Body.pivot = UiViewExtensions.GetPivotFromDirectionTarget(pivotTarget);
            hintView.Body.transform.position = hintPosition;
            
            hintView.Pointer.rotation = Quaternion.Euler(0f, 0f, GetPointerAngle(pivotTarget));
            hintView.Pointer.transform.position = hintPosition;

            View.HintSimpleView.TextDescription.text = description;

            return View.HintSimpleView;
        }

        private Vector3 GetHintOffsetWithPointer(EPivotTarget pivotTarget, RectTransform pointerRectTransform)
        {
            var scaleFactor = _canvasProvider.Canvas.scaleFactor;
            switch (pivotTarget)
            {
                case EPivotTarget.Left:
                    return Vector3.right * pointerRectTransform.rect.height * scaleFactor;
                case EPivotTarget.Right:
                    return Vector3.left * pointerRectTransform.rect.height * scaleFactor;
                case EPivotTarget.Up:
                    return Vector3.down * pointerRectTransform.rect.height * scaleFactor;
                case EPivotTarget.Down:
                    return Vector3.up * pointerRectTransform.rect.height * scaleFactor;
                case EPivotTarget.Center:
                    return Vector3.zero;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pivotTarget), pivotTarget, null);
            }
        }

        private float GetPointerAngle(EPivotTarget pivotTarget)
        {
            switch (pivotTarget)
            {
                case EPivotTarget.Left:
                    return 90;
                case EPivotTarget.Right:
                    return 270;
                case EPivotTarget.Up:
                    return 0;
                case EPivotTarget.Down:
                    return 180;
                case EPivotTarget.Center:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pivotTarget), pivotTarget, null);
            }
        }
    }
}