using System;
using DG.Tweening;
using Libs.OpenUI;
using Libs.OpenUI.UiEffects;
using Models.Player;
using SampleScene.UiViews.Views;
using UniRx;

namespace SampleScene.UiViews.Presenters
{
    public class UiTopLeftViewPresenter : UiPresenter<UiTopLeftView>
    {
        private readonly IPlayerService _playerService;
        private readonly IPlayerSetting _playerSetting;

        public UiTopLeftViewPresenter(IPlayerService playerService, IPlayerSetting playerSetting)
        {
            _playerService = playerService;
            _playerSetting = playerSetting;
        }

        public override void Initialize()
        {
            base.Initialize();

            // Reactive Notification Model
            // https://github.com/neuecc/UniRx#reactiveproperty-reactivecollection
            _playerService.Player.Health.SubscribeToText(View.HealthData.TextValue);

            _playerService.Player.Health
                .Subscribe(UpdateHealth)
                .AddTo(Disposables);

            _playerService.OnXpUpdate
                .Subscribe(UpdateXp)
                .AddTo(Disposables);

            View.HealthData.TextValue.text = _playerService.Player.Health.Value.ToString();
            View.XpData.TextValue.text = _playerService.Player.Xp.Value.ToString();
        }

        public override void ShowWithAnimation(Action complete = null)
        {
            View.ExpandAnimation(View.Body, EAnimationTarget.Down);
        }

        public override void HideWithAnimation(Action complete = null)
        {
            View.CollapseAnimation(View.Body, EAnimationTarget.Up);
        }

        private void UpdateHealth(int value)
        {
            var endValue = (float) value / _playerSetting.MaxHealth;
            DOTween.To(() => View.HealthData.Slider.value, x => View.HealthData.Slider.value = x, endValue, 1f);
        }

        private void UpdateXp(OnXpUpdateData data)
        {
            if (data.DifferenceXp < data.NextLevelXpBound)
            {
                View.XpData.TextValue.text = $"{data.DifferenceXp}/{data.NextLevelXpBound}";

                var endValue = (float) data.DifferenceXp / data.NextLevelXpBound;
                DOTween.To(() => View.XpData.Slider.value, x => View.XpData.Slider.value = x, endValue, 1f);
            }
            else
            {
                View.XpData.TextValue.text = data.DifferenceXp.ToString();
                View.XpData.Slider.value = 1f;
            }
        }
    }
}