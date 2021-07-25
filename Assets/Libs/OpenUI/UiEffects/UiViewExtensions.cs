using System;
using Crystal;
using DG.Tweening;
using Libs.OpenUI.Signals;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Libs.OpenUI.UiEffects
{
    public static class UiViewExtensions
    {
        private const float ExpandTime = 0.4f;
        private const float FadeTime = 0.5f;

        public static void FadeIn(this UiView view, RectTransform rectTransform, CanvasGroup canvasGroup,
            Action complete = null)
        {
            canvasGroup.alpha = 0f;
            var sequence = DOTween.Sequence();
            sequence.Append(rectTransform.DOPunchScale(Vector3.one * 0.05f, FadeTime)).SetUpdate(true);
            sequence.Join(canvasGroup.DOFade(1f, FadeTime)).SetUpdate(true);
            sequence.OnComplete(() => { complete?.Invoke(); });
        }

        public static void FadeOut(this UiView view, CanvasGroup canvasGroup,
            Action complete = null)
        {
            canvasGroup.DOFade(0f, FadeTime / 3f).OnComplete(() => { complete?.Invoke(); }).SetUpdate(true);
        }

        public static void FadeOut(this UiView view, CanvasGroup canvasGroup, float fadeTime,
            Action complete = null)
        {
            canvasGroup.DOFade(0f, fadeTime).OnComplete(() => { complete?.Invoke(); }).SetUpdate(true);
        }

        public static void EnableButtonEffects(this UiView view)
        {
            foreach (var button in view.gameObject.GetComponentsInChildren<Button>())
            {
                AddEffectOnButton(button);
            }
        }

        public static void AddSignalClick(this UiView view, SignalBus signalBus)
        {
            foreach (var button in view.GetComponentsInChildren<Button>())
            {
                button.OnClickAsObservable()
                    .Subscribe(_ => signalBus.Fire(new SignalButtonClick(button.GetComponent<RectTransform>())))
                    .AddTo(view);
            }

            foreach (var toggle in view.GetComponentsInChildren<Toggle>())
            {
                toggle.OnPointerClickAsObservable()
                    .Subscribe(_ => signalBus.Fire(new SignalButtonClick(toggle.GetComponent<RectTransform>())))
                    .AddTo(view);
            }
        }

        static void AddEffectOnButton(Button button)
        {
            button.OnPointerClickAsObservable()
                .Subscribe(x =>
                {
                    if (!button.enabled)
                        return;

                    button.transform.localScale = Vector3.one;
                    button.transform.DOPunchScale(Vector3.one * 0.05f, 0.5f).OnComplete(() =>
                    {
                        button.transform.localScale = Vector3.one;
                    });
                });
        }

        public static void CollapseAnimation(this UiView view, RectTransform rectTransform, EAnimationTarget target,
            Action complete = null, Ease ease = Ease.InBack)
        {
            var with = rectTransform.rect.size.x;
            var height = rectTransform.rect.size.y;
            var endValue = rectTransform.anchoredPosition;

            var sequence = DOTween.Sequence();
            switch (target)
            {
                case EAnimationTarget.Left:
                    endValue.x = -(with + with * 0.3f);
                    break;
                case EAnimationTarget.Right:
                    endValue.x = with + with * 0.3f;
                    break;
                case EAnimationTarget.Up:
                    endValue.y = height;
                    break;
                case EAnimationTarget.Down:
                    endValue.y = -height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }

            sequence.Append(rectTransform.DOAnchorPos(endValue, ExpandTime).SetEase(ease));
            sequence.OnComplete(() => { complete?.Invoke(); });
        }

        public static void ExpandAnimation(this UiView view, RectTransform rectTransform, EAnimationTarget target,
            Action complete = null, Ease ease = Ease.OutBack)
        {
            var endValue = rectTransform.anchoredPosition;
            var with = rectTransform.rect.size.x;

            var safeArea = view.GetComponentInChildren<SafeArea>();
            if (safeArea)
            {
                var nsa = safeArea.GetNsa();
                with += nsa.x;
            }

            switch (target)
            {
                case EAnimationTarget.Left:
                    endValue.x = 0f;
                    rectTransform.anchoredPosition = new Vector2(with, endValue.y);
                    break;
                case EAnimationTarget.Right:
                    endValue.x = 0f;
                    with = -with;
                    rectTransform.anchoredPosition = new Vector2(with, endValue.y);
                    break;
                case EAnimationTarget.Up:
                    endValue.y = 0f;
                    break;
                case EAnimationTarget.Down:
                    endValue.y = 0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }

            var sequence = DOTween.Sequence();
            sequence.Append(rectTransform.DOAnchorPos(endValue, ExpandTime).SetEase(ease));

            sequence.OnComplete(() => { complete?.Invoke(); });
        }

        public static Vector2 GetPivotFromDirectionTarget(EPivotTarget pivotTarget)
        {
            switch (pivotTarget)
            {
                case EPivotTarget.Left:
                    return new Vector2(0f, 0.5f);
                case EPivotTarget.Right:
                    return new Vector2(1f, 0.5f);
                case EPivotTarget.Up:
                    return new Vector2(0.5f, 1f);
                case EPivotTarget.Down:
                    return new Vector2(0.5f, 0f);
                case EPivotTarget.Center:
                    return new Vector2(0.5f, 0.5f);
                default:
                    throw new ArgumentOutOfRangeException(nameof(pivotTarget), pivotTarget, null);
            }
        }

        public static Vector3 GetHintPosition(RectTransform targetRect, EPivotTarget pivotTarget, float scaleFactor)
        {
            var offset = Vector3.zero;

            switch (pivotTarget)
            {
                case EPivotTarget.Left:
                    offset = Vector3.left * targetRect.rect.width * 0.5f * scaleFactor;
                    break;
                case EPivotTarget.Right:
                    offset = Vector3.right * targetRect.rect.width * 0.5f * scaleFactor;
                    break;
                case EPivotTarget.Up:
                    offset = Vector3.up * targetRect.rect.height * 0.5f * scaleFactor;
                    break;
                case EPivotTarget.Down:
                    offset = Vector3.down * targetRect.rect.height * 0.5f * scaleFactor;
                    break;
                case EPivotTarget.Center:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pivotTarget), pivotTarget, null);
            }

            var pivot = targetRect.pivot;

            var offsetY = Mathf.Lerp(1f, -1f, pivot.y);
            var offsetX = Mathf.Lerp(1f, -1f, pivot.x);
            var realCenter = targetRect.position 
                             + Vector3.up * targetRect.rect.height * 0.5f * offsetY * scaleFactor
                             + Vector3.right * targetRect.rect.width * 0.5f * offsetX * scaleFactor;
            return realCenter + offset;
        }

        public static void Pulsating(this RectTransform targetRect, int count, Action complete = null)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(targetRect.DOScale(1.1f, 0.2f));
            sequence.Append(targetRect.DOScale(1f, 0.3f));
            sequence.OnComplete(() =>
            {
                --count;
                if (count > 0)
                    Pulsating(targetRect, count, complete);
                else
                    complete?.Invoke();
            });
        }

        public static void ButtonPressAnim(Image imageActionRoot)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(imageActionRoot.transform.DOScale(0.9f, 0.2f));
            sequence.Append(imageActionRoot.transform.DOScale(1f, 0.1f));
        }

        #region FlipAndBack

        private const float DurationFlipAnimation = 0.2f;
        private const float DurationShowRootBack = 3f;

        public static void FlipAndBack(this FlipRoot flipRoot)
        {
            if (flipRoot.IsInFlipAnimationProcess)
                return;

            flipRoot.IsInFlipAnimationProcess = true;

            if (flipRoot.IsFlipBackAnimationFlag)
            {
                RunAnimationFlipBackToFront(flipRoot);
            }
            else
            {
                StartBackFlipTimer(flipRoot);
                RunAnimationFlipFrontToBack(flipRoot);
            }
        }

        private static void RunAnimationFlipBackToFront(FlipRoot view)
        {
            FlipBackToFront(view, () =>
            {
                view.IsInFlipAnimationProcess = false;
                view.IsFlipBackAnimationFlag = false;
            });
        }

        private static void RunAnimationFlipFrontToBack(FlipRoot view)
        {
            FlipFrontToBack(view, () =>
            {
                view.IsInFlipAnimationProcess = false;
                view.IsFlipBackAnimationFlag = true;
            });
        }

        private static void StartBackFlipTimer(FlipRoot view)
        {
            Observable.Timer(TimeSpan.FromSeconds(DurationShowRootBack))
                .Subscribe(_ =>
                {
                    if (view.IsInFlipAnimationProcess)
                        return;

                    if (view.IsFlipBackAnimationFlag)
                    {
                        RunAnimationFlipBackToFront(view);
                    }
                });
        }

        private static void FlipFrontToBack(FlipRoot view, Action complete)
        {
            view.RootFront.DOScaleY(0f, DurationFlipAnimation).OnComplete(() =>
            {
                view.RootBack.gameObject.SetActive(true);
                view.RootFront.gameObject.SetActive(false);
                view.RootBack.localScale = new Vector3(1f, 0f, 1f);
                view.RootBack.DOScaleY(1f, DurationFlipAnimation).OnComplete(() => { complete?.Invoke(); });
            });
        }

        private static void FlipBackToFront(FlipRoot view, Action complete)
        {
            view.RootBack.DOScaleY(0f, DurationFlipAnimation).OnComplete(() =>
            {
                view.RootFront.gameObject.SetActive(true);
                view.RootBack.gameObject.SetActive(false);
                view.RootFront.localScale = new Vector3(1f, 0f, 1f);
                view.RootFront.DOScaleY(1f, DurationFlipAnimation).OnComplete(() => { complete?.Invoke(); });
            });
        }

        #endregion
    }
}