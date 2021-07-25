using System;
using Game.Providers;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using ProjectContext.Providers;
using SampleScene.UiViews.Views;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Presenters
{
    public class UiTopCenterViewPresenter : UiPresenter<UiTopCenterView>
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ILocalizationProvider _localization;
        private readonly IUiHintsViewPresenter _uiHintsViewPresenter;
        private readonly ICanvasProvider _canvasProvider;
        private IDisposable _uiUpdater;
        private IDisposable _hintDisposable;
        private UiView _hint;

        public UiTopCenterViewPresenter(ITimeProvider timeProvider, ILocalizationProvider localization,
            IUiHintsViewPresenter uiHintsViewPresenter, ICanvasProvider canvasProvider)
        {
            _timeProvider = timeProvider;
            _localization = localization;
            _uiHintsViewPresenter = uiHintsViewPresenter;
            _canvasProvider = canvasProvider;
        }

        public override void Initialize()
        {
            base.Initialize();
            

            AddHint(View.HintArea, "HintDescription");

            OnShow
                .Subscribe(_ =>
                {
                    _uiUpdater = Observable.Interval(TimeSpan.FromSeconds(UiConstants.UiUpdateTime))
                        .Subscribe(_ => UpdateLocalTime());
                })
                .AddTo(Disposables);

            OnHide
                .Subscribe(_ => { _uiUpdater?.Dispose(); })
                .AddTo(Disposables);
            
            Show();
            UpdateLocalTime();
        }

        public override void ShowWithAnimation(Action complete = null)
        {
            View.ExpandAnimation(View.Body, EAnimationTarget.Down);
        }

        public override void HideWithAnimation(Action complete = null)
        {
            View.CollapseAnimation(View.Body, EAnimationTarget.Up);
        }

        private void UpdateLocalTime()
        {
            //localization template
            var time = _timeProvider.GetUtcTime.ToString("HH:mm:ss");
            var localizationTime = _localization.Get("TimeTemplate", time);
            View.TextLocalTime.text = localizationTime;
        }

        private void AddHint(Image hintArea, string keyLocalization)
        {
            hintArea.OnPointerDownAsObservable()
                .Subscribe(_ =>
                {
                    _hintDisposable = Observable.Timer(TimeSpan.FromSeconds(UiConstants.HintPressedTime))
                        .Subscribe(_ =>
                        {
                            var hintDescription = Translate(keyLocalization);
                            var pivotTarget = EPivotTarget.Down;
                            var hintPosition =
                                UiViewExtensions.GetHintPosition(View.Body, pivotTarget, _canvasProvider.Canvas.scaleFactor);
                          
                            _hint = _uiHintsViewPresenter.OpenSimpleHint(hintDescription, hintPosition,
                                EPivotTarget.Up);
                            
                        }).AddTo(hintArea);
                   
                })
                .AddTo(hintArea);

            hintArea.OnPointerUpAsObservable()
                .Subscribe(_ => { HideHints(); })
                .AddTo(hintArea);

            hintArea.OnPointerExitAsObservable()
                .Subscribe(_ => { HideHints(); })
                .AddTo(hintArea);
        }

        private void HideHints()
        {
            _hintDisposable?.Dispose();
            if (_hint != null)
            {
                _uiHintsViewPresenter.CloseHint(_hint);
                _hint = null;
            }
        }
    }
}