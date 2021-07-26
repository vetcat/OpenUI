using System;
using DG.Tweening;
using EasyButtons;
using Libs.OpenUI;
using Libs.OpenUI.Utils;
using ProjectContext.Settings;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SampleScene.UiViews.Views.UiFx
{
    public class UiFxView : UiView
    {
        public UiFxItemView FxItemView;
        private TemplatePool<UiFxItemView> _poolFxItem;
        
        private const int PoolCount = 10;
        private const float FadeDuration = 0.2f;
        private const float AnimationInstanceInterval = 0.07f;
        private const float InitialPositionRadius = 7f;
        private Color _startAmountColor;

        //todo for test
        public RectTransform TestRectTransformSource;
        public RectTransform TestRectTransformTarget;

        private void Awake()
        {
            _startAmountColor = FxItemView.TextAmount.color;
            _poolFxItem = new TemplatePool<UiFxItemView>(FxItemView, FxItemView.transform.parent, PoolCount);
        }

        [Button]
        private void TestCollectSimpleAnimation()
        {
            CollectSimpleAnimation(50, null, TestRectTransformSource.transform.position, TestRectTransformTarget,
                () => { Debug.Log("CollectAnimation complete"); });
        }

        [Button]
        private void TestCollectAdvanceAnimation()
        {
            CollectAdvanceAnimation(50, null, TestRectTransformSource, TestRectTransformTarget,
                () => { Debug.Log("CollectAnimation complete"); });
        }

        [Button]
        private void TestSpendingAnimation()
        {
            SpendingAnimation(50, null, TestRectTransformSource.transform.position,
                () => { Debug.Log("CollectAnimation complete"); });
        }

        public void CollectSimpleAnimation(int amount, Sprite icon, Vector3 startPosition,
            RectTransform rectTransformTo, Action complete = null)
        {
            var sequence = DOTween.Sequence();

            AddFlySimpleAnimation(sequence, amount, icon, _poolFxItem, startPosition,
                rectTransformTo);

            sequence.OnComplete(() =>
            {
                complete?.Invoke();
            });
        }
        
        public void CollectSimpleAnimation(int amount, Sprite icon, RectTransform rectTransformFrom,
            RectTransform rectTransformTo, Action complete = null)
        {
            var sequence = DOTween.Sequence();

            AddFlySimpleAnimation(sequence, amount, icon, _poolFxItem, rectTransformFrom.position,
                rectTransformTo);

            sequence.OnComplete(() => { complete?.Invoke(); });
        }

        public void SpendingAnimation(int amount, Sprite icon, Vector3 startPosition, Action complete = null)
        {
            var sequence = DOTween.Sequence();

            SpendingAnimation(sequence, amount, icon, _poolFxItem, startPosition);

            sequence.OnComplete(() => { complete?.Invoke(); });
        }

        public void CollectAdvanceAnimation(int rewardCount, Sprite icon, RectTransform rectTransformFrom,
            RectTransform rectTransformTo, Action complete = null)
        {
            var sequence = DOTween.Sequence();
            if (rewardCount > PoolCount)
                rewardCount = PoolCount;

            for (var i = 0; i <= rewardCount; i++)
            {
                var animatedSequence = DOTween.Sequence();
                var flyAdvanceAnimation =
                    AddFlyAdvanceAnimation(i, icon, _poolFxItem, rectTransformFrom, rectTransformTo);
                //add more if need ...
                sequence.Insert(i * AnimationInstanceInterval, flyAdvanceAnimation);
            }

            sequence.OnComplete(() => { complete?.Invoke(); });
        }

        private Sequence AddFlySimpleAnimation(Sequence sequence, int amount, Sprite icon,
            TemplatePool<UiFxItemView> pool,
            Vector3 startPosition, RectTransform rectTransformTo)
        {
            var rewardTemplate = pool.GetFirstAvailable();
            PreparePoolObject(rewardTemplate, startPosition);
            if (icon != null)
                rewardTemplate.ImageIcon.sprite = icon;
            rewardTemplate.TextAmount.text = $"+{amount}";

            var initialPosition = startPosition;
            sequence.Append(rewardTemplate.transform.DOMove(initialPosition, 0.05f).SetEase(Ease.OutCubic));
            sequence.Append(rewardTemplate.ImageIcon.DOFade(1f, FadeDuration));
            sequence.Join(rewardTemplate.TextAmount.DOFade(1f, FadeDuration));
            sequence.Join(rewardTemplate.transform.DOScale(1.2f, FadeDuration * .5f)
                .SetEase(Ease.OutBack));

            sequence.Append(rewardTemplate.Body.DOMove(rectTransformTo.position, 2f)).SetEase(Ease.InOutSine);
            sequence.Join(rewardTemplate.TextAmount.transform.DOScale(0.9f, 1f));
            sequence.Join(rewardTemplate.TextAmount.DOColor(Color.white, 1f));

            var fade = DOTween.Sequence();
            fade.Append(rewardTemplate.ImageIcon.DOFade(0f, 0.2f));
            fade.Join(rewardTemplate.TextAmount.transform.DOScale(0.8f, 0.2f));
            fade.Join(rewardTemplate.TextAmount.DOFade(0f, 0.2f));

            sequence.Insert(sequence.Duration() - 0.4f, fade);
            sequence.OnComplete(() => { });

            fade.OnComplete(() => { rewardTemplate.gameObject.SetActive(false); });

            return sequence;
        }

        private Sequence SpendingAnimation(Sequence sequence, int amount, Sprite icon, TemplatePool<UiFxItemView> pool,
            Vector3 startPosition)
        {
            var rewardTemplate = pool.GetFirstAvailable();
            PreparePoolObject(rewardTemplate, startPosition);
            rewardTemplate.TextAmount.color = Color.red;
            if (icon != null)
                rewardTemplate.ImageIcon.sprite = icon;
            rewardTemplate.TextAmount.text = $"-{amount}";

            var initialPosition =
                AnimationUtils.GetInitialGoldenRatioPosition(startPosition, Random.Range(10,20), InitialPositionRadius);
            sequence.Append(rewardTemplate.transform.DOMove(initialPosition, 0.5f).SetEase(Ease.OutCubic));
            //sequence.Join(rewardTemplate.transform.DOScale(1.6f, 0.5f));
            sequence.Join(rewardTemplate.ImageIcon.DOFade(1f, FadeDuration));
            sequence.Join(rewardTemplate.TextAmount.DOFade(1f, FadeDuration));

            sequence.Append(rewardTemplate.transform.DOScale(0.5f, 0.5f));
            sequence.Join(rewardTemplate.ImageIcon.DOFade(0f, 0.2f));
            sequence.Join(rewardTemplate.TextAmount.DOFade(0f, 0.2f));
            sequence.AppendCallback(() =>
            {
                rewardTemplate.gameObject.SetActive(false);
            });

            return sequence;
        }


        private Sequence AddFlyAdvanceAnimation(int i, Sprite icon, TemplatePool<UiFxItemView> pool, RectTransform rectTransformFrom,
            RectTransform rectTransformTo)
        {
            var sequence = DOTween.Sequence();

            var rewardTemplate = pool.GetFirstAvailable();
            PreparePoolObject(rewardTemplate, rectTransformFrom.transform.localPosition);
            if (icon != null)
                rewardTemplate.ImageIcon.sprite = icon;

            var initialPosition =
                AnimationUtils.GetInitialGoldenRatioPosition(rectTransformFrom.position, i, InitialPositionRadius);
            sequence.Append(rewardTemplate.transform.DOMove(initialPosition, 0.2f).SetEase(Ease.OutCubic));
            sequence.Append(rewardTemplate.ImageIcon.DOFade(1f, FadeDuration));

            sequence.Append(rewardTemplate.Body.DOMove(rectTransformTo.position, 0.4f));
            sequence.Join(rewardTemplate.transform.DOScale(0.6f, 0.5f));
            sequence.Join(rewardTemplate.ImageIcon.DOFade(0f, 0.2f).SetDelay(FadeDuration));
            sequence.AppendCallback(() =>
            {
                rewardTemplate.gameObject.SetActive(false);
            });

            return sequence;
        }

        private void PreparePoolObject(Image image, Vector3 startPosition)
        {
            image.SetAlpha(0f);
            image.transform.localPosition = startPosition;
            image.gameObject.SetActive(true);
        }

        private void PreparePoolObject(UiFxItemView item, Vector3 startPosition)
        {
            item.ImageIcon.SetAlpha(0f);
            item.TextAmount.color = _startAmountColor;
            item.TextAmount.SetAlpha(0f);
            item.TextAmount.transform.localScale = Vector3.one;
            item.transform.localScale = Vector3.one;
            item.transform.position = startPosition;
            item.gameObject.SetActive(true);
        }
    }
}