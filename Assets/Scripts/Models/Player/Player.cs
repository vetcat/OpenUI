using UniRx;

namespace Models.Player
{
    // Reactive Notification Model
    // https://github.com/neuecc/UniRx#reactiveproperty-reactivecollection
    public class Player : IPlayer
    {
        public ReactiveProperty<int> Health { get; }
        public ReactiveProperty<int> Xp { get; }
        public ReactiveProperty<int> Level { get; }
        public ReactiveCommand UpdatePlayerReactiveCommand { get; }

        public Player(IPlayerSetting playerSetting)
        {
            Xp = new ReactiveProperty<int>(0);
            Health = new ReactiveProperty<int>(playerSetting.MaxHealth);
            Level = new ReactiveProperty<int>(1);
        }
        
        public void SetXp(int value)
        {
            Xp.Value = value;
            UpdatePlayerReactiveCommand.Execute();
        }
        
        public void SetHealth(int value)
        {
            Xp.Value = value;
            UpdatePlayerReactiveCommand.Execute();
        }
        
        public void SetLevel(int value)
        {
            Level.Value = value;
            UpdatePlayerReactiveCommand.Execute();
        }
    }
}