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

        public Player()
        {
            Xp = new ReactiveProperty<int>(0);
            Health = new ReactiveProperty<int>(100);
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