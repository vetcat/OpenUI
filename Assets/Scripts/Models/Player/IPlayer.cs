using UniRx;

namespace Models.Player
{
    public interface IPlayer
    {
        ReactiveProperty<int> Health { get; }
        ReactiveProperty<int> Xp { get; }
        ReactiveProperty<int> Level { get; }
        ReactiveCommand UpdatePlayerReactiveCommand { get; }

        void SetXp(int value);
        void SetHealth(int value);
        void SetLevel(int value);
    }
}