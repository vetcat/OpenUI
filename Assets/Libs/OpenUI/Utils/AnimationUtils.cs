using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Libs.OpenUI.Utils
{
    public static class AnimationUtils
    {
        private const int MaxAmountAnimationCount = 10;

        public static Vector3 GetInitialGoldenRatioPosition(Vector3 startPositionCoordinates, int i,
            float initialPositionRadius)
        {
            const float goldenRatio = 1.618f;
            const float oneOverGoldenRatio = 1f / goldenRatio;

            var circleAngle = Random.Range(0f, Mathf.PI * 2f);
            var radius = initialPositionRadius;

            circleAngle += i * goldenRatio;
            radius = radius - radius * oneOverGoldenRatio * i;

            var x = radius * Mathf.Cos(circleAngle);
            var y = radius * Mathf.Sin(circleAngle);
            var initialCirclePosition = new Vector3(x, y, 0f);
            return initialCirclePosition + startPositionCoordinates;
        }

        public static void SetUiTextAnimation(Text text, int initValue, int amount,
            Action complete = null)
        {
            var sequence = DOTween.Sequence();
            var count = Math.Abs(amount);

            if (count > MaxAmountAnimationCount)
                count = MaxAmountAnimationCount;

            for (var i = 0; i < count; i++)
            {
                var current = i;
                if (amount < 0)
                    current = -i;

                var animatedSequence = DOTween.Sequence();
                animatedSequence = AddUiTextAnimationAmount(animatedSequence, current, initValue, text);
                sequence.Insert(i * 0.1f, animatedSequence);
            }

            sequence.OnComplete(() =>
            {
                text.text = (initValue + amount).ToString();
                complete?.Invoke();
            });
        }

        private static Sequence AddUiTextAnimationAmount(Sequence sequence, int i, int initialAmount,
            Text target)
        {
            sequence.AppendCallback(() => { target.text = (initialAmount + i).ToString(); });
            return sequence;
        }

        public static void SetPropertyAnimation(ReactiveProperty<int> property, int initValue, int amount,
            Action complete = null)
        {
            var sequence = DOTween.Sequence();
            var count = Math.Abs(amount);

            if (count > MaxAmountAnimationCount)
                count = MaxAmountAnimationCount;

            for (var i = 0; i < count; i++)
            {
                var current = i;
                if (amount < 0)
                    current = -i;

                var animatedSequence = DOTween.Sequence();
                animatedSequence = AddAnimationAmount(animatedSequence, current, initValue, property);
                sequence.Insert(i * 0.15f, animatedSequence);
            }

            sequence.OnComplete(() =>
            {
                property.Value = initValue + amount;
                complete?.Invoke();
            });
        }

        private static Sequence AddAnimationAmount(Sequence sequence, int i, int initialAmount,
            ReactiveProperty<int> target)
        {
            sequence.AppendCallback(() => { target.Value = initialAmount + i; });
            return sequence;
        }


        public static void IntervalCallBack(int count, float interval, Action callback, Action complete)
        {
            var sequence = DOTween.Sequence();
            for (var i = 0; i < count; i++)
            {
                var animatedSequence = DOTween.Sequence();
                animatedSequence.AppendCallback(() => { callback?.Invoke(); });
                sequence.Insert(i * interval, animatedSequence);

                sequence.OnComplete(() => complete?.Invoke());
            }
        }
        
        public static TimeSpan UpdateAnimatedLeftTime(TimeSpan leftTime, TimeSpan endTime)
        {
            if (leftTime.Days > endTime.Days)
                leftTime -= TimeSpan.FromDays(1);
            if (leftTime.Hours > endTime.Hours)
                leftTime -= TimeSpan.FromHours(1);
            if (leftTime.Minutes > endTime.Minutes)
                leftTime -= TimeSpan.FromMinutes(1);
            if (leftTime.Seconds > 0 && leftTime.Minutes > 0)
                leftTime -= TimeSpan.FromSeconds(5);
            else if (leftTime.Seconds > endTime.Seconds)
            {
                leftTime -= TimeSpan.FromSeconds(1);
            }
            
            return leftTime;
        }
    }
}