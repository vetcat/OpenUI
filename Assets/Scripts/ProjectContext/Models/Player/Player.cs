using UniRx;
using UnityEngine;

namespace ProjectContext.Models.Player
{
    // Reactive Notification Model
    // https://github.com/neuecc/UniRx#reactiveproperty-reactivecollection
    public class Player : IPlayer
    {
        private readonly IPlayerSetting _playerSetting;
        public ReactiveProperty<int> Health { get; }
        public ReactiveProperty<int> Xp { get; }
        public ReactiveProperty<int> Level { get; }
        public ReactiveProperty<string> Name { get; }
        
        public ReactiveCommand UpdatePlayerReactiveCommand { get; }

        public Player(IPlayerSetting playerSetting)
        {
            _playerSetting = playerSetting;
            Xp = new ReactiveProperty<int>(0);
            Health = new ReactiveProperty<int>(playerSetting.MaxHealth);
            Level = new ReactiveProperty<int>(1);
            Name = new ReactiveProperty<string>("Default name");
            UpdatePlayerReactiveCommand = new ReactiveCommand();
        }
        
        public void SetXp(int value)
        {
            Xp.Value = value;
            UpdatePlayerReactiveCommand.Execute();
        }
        
        public void SetHealth(int value)
        {
            value = Mathf.Clamp(value, 0, _playerSetting.MaxHealth);
            Health.Value = value;
            UpdatePlayerReactiveCommand.Execute();
        }
        
        public void SetLevel(int value)
        {
            Level.Value = value;
            UpdatePlayerReactiveCommand.Execute();
        }
        
        public void SetName(string value)
        {
            Name.Value = value;
            UpdatePlayerReactiveCommand.Execute();
        }
    }
}