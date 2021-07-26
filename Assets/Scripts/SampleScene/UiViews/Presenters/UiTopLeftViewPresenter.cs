using System;
using DG.Tweening;
using Libs.OpenUI;
using Libs.OpenUI.UiEffects;
using ProjectContext.Models.Player;
using SampleScene.UiViews.Views;
using UniRx;
using UnityEngine;

namespace SampleScene.UiViews.Presenters
{
    public class UiTopLeftViewPresenter : UiPresenter<UiTopLeftView>
    {
        private readonly IPlayerService _playerService;
        private readonly IPlayerSetting _playerSetting;
        private readonly IUiShopViewPresenter _uiShopViewPresenter;

        public UiTopLeftViewPresenter(IPlayerService playerService, IPlayerSetting playerSetting,
            IUiShopViewPresenter uiShopViewPresenter)
        {
            _playerService = playerService;
            _playerSetting = playerSetting;
            _uiShopViewPresenter = uiShopViewPresenter;
        }

        public override void Initialize()
        {
            base.Initialize();

            // Reactive Notification Model
            // https://github.com/neuecc/UniRx#reactiveproperty-reactivecollection
            _playerService.Player.Health.SubscribeToText(View.HealthData.TextValue);
            _playerService.Player.Name.SubscribeToText(View.TextPlayerName);
            _playerService.Player.Level.SubscribeToText(View.TextLevelValue);

            _playerService.Player.Health
                .Subscribe(UpdateHealth)
                .AddTo(Disposables);

            _playerService.OnXpUpdate
                .Subscribe(UpdateXp)
                .AddTo(Disposables);

            _playerService.OnLevelUp
                .Subscribe(LevelUp)
                .AddTo(Disposables);

            _uiShopViewPresenter.OnShow
                .Subscribe(_ => HideWithAnimation())
                .AddTo(Disposables);

            _uiShopViewPresenter.OnHide
                .Subscribe(_ => ShowWithAnimation())
                .AddTo(Disposables);

            //direct set
            View.HealthData.TextValue.text = _playerService.Player.Health.Value.ToString();
            View.XpData.TextValue.text = _playerService.Player.Xp.Value.ToString();
            View.TextPlayerName.text = _playerService.Player.Name.ToString();
            View.TextLevelValue.text = _playerService.Player.Level.ToString();

            //simple logic can be written directly in the subscription 
            View.HealthData.ButtonAdd.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    if (_playerService.Player.Health.Value < _playerSetting.MaxHealth)
                    {
                        var delta = _playerSetting.MaxHealth / 5;
                        _playerService.Player.SetHealth(_playerService.Player.Health.Value + delta);
                    }
                })
                .AddTo(Disposables);

            View.HealthData.ButtonReduce.OnClickAsObservable()
                .Subscribe(_ => { _playerService.Player.SetHealth(_playerService.Player.Health.Value / 2); })
                .AddTo(Disposables);

            View.XpData.ButtonAdd.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _playerService.GetLevelByXp(out var nextLevelBound);
                    var delta = nextLevelBound / 5;
                    _playerService.Player.SetXp(_playerService.Player.Xp.Value + delta);
                })
                .AddTo(Disposables);
        }

        public override void ShowWithAnimation(Action complete = null)
        {
            Show();
            View.ExpandAnimation(View.Body, EAnimationTarget.Down, complete);
        }

        public override void HideWithAnimation(Action complete = null)
        {
            View.CollapseAnimation(View.Body, EAnimationTarget.Up, () => { base.HideWithAnimation(complete); });
        }

        private void UpdateHealth(int value)
        {
            var endValue = (float) value / _playerSetting.MaxHealth;
            DOTween.To(() => View.HealthData.Slider.value, x => View.HealthData.Slider.value = x, endValue, 0.5f)
                .SetEase(Ease.Linear);

            View.HealthData.ImageIcon.GetComponent<RectTransform>().Pulsating(2);
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

        private void LevelUp(int value)
        {
            View.XpData.ImageIcon.GetComponent<RectTransform>().Pulsating(3);
            View.TextLevelValue.GetComponent<RectTransform>().Pulsating(3);
        }
    }
}