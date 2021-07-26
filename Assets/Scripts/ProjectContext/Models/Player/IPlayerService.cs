using System;
using UnityEngine;

namespace ProjectContext.Models.Player
{
    public interface IPlayerService
    {
        IObservable<int> OnLevelUp { get; }
        public IObservable<OnXpUpdateData> OnXpUpdate { get; }
        IPlayer Player { get; }
        int GetDifferenceXp();
        int GetLevelByXp(out int nextLevelBound);

        void AddCoinsWithAnimation(int amount, Vector3 effectPosition,
            bool convertWorldToScreenPoint = true);

        void AddCoinsWithAnimation(int amount, RectTransform rectSource);

        void RemoveCoinsWithAnimation(int amount, Vector3 effectPosition,
            bool convertWorldToScreenPoint = true);
    }
}