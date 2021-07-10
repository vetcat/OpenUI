using System;

namespace Models.Player
{
    public interface IPlayerService
    {
        IObservable<int> OnLevelUp { get; }
        public IObservable<OnXpUpdateData> OnXpUpdate { get; }
        IPlayer Player { get; }
        int GetDifferenceXp();
        int GetLevelByXp(out int nextLevelBound);
    }
}