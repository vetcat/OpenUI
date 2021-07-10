using System;
using Libs.OpenUI;
using Libs.OpenUI.UiEffects;
using Models.Player;
using SampleScene.UiViews.Views;
using UniRx;

namespace SampleScene.UiViews.Presenters
{
    public class UiTopLeftViewPresenter : UiPresenter<UiTopLeftView>
    {
        private readonly IPlayer _player;

        public UiTopLeftViewPresenter(IPlayer player)
        {
            _player = player;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            // Reactive Notification Model
            // https://github.com/neuecc/UniRx#reactiveproperty-reactivecollection
            _player.Health.SubscribeToText(View.HealthData.TextValue);
            _player.Xp.SubscribeToText(View.XpData.TextValue);

            _player.Health
                .Subscribe(UpdateHealth)
                .AddTo(Disposables);
            
            _player.Xp
                .Subscribe(UpdateXp)
                .AddTo(Disposables);
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
            
        }
        
        private void UpdateXp(int value)
        {
            
        }
    }
}