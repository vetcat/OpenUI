using System;
using ProjectContext.Settings;
using SampleScene.Providers;
using SampleScene.UiViews.Presenters;
using UniRx;
using UnityEngine;
using Zenject;

namespace ProjectContext.Models.Player
{
    public readonly struct OnXpUpdateData
    {
        public readonly int DifferenceXp;
        public readonly int NextLevelXpBound;

        public OnXpUpdateData(int differenceXp, int nextLevelXpBound)
        {
            DifferenceXp = differenceXp;
            NextLevelXpBound = nextLevelXpBound;
        }
    }

    public class PlayerService : IPlayerService, IInitializable, IDisposable
    {
        public IObservable<int> OnLevelUp => _onLevelUp;
        public IObservable<OnXpUpdateData> OnXpUpdate => _onXpUpdate;
        public IPlayer Player => _player;

        private readonly Subject<int> _onLevelUp;
        private readonly Subject<OnXpUpdateData> _onXpUpdate;

        private readonly IPlayer _player;
        private readonly IPlayerSetting _playerSetting;
        private readonly ICameraProvider _cameraProvider;
        private readonly IUiFxViewPresenter _fxViewPresenter;
        private readonly IconSettings _iconSettings;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public PlayerService(IPlayer player, IPlayerSetting playerSetting, ICameraProvider cameraProvider,
            IUiFxViewPresenter fxViewPresenter, IconSettings iconSettings)
        {
            _onLevelUp = new Subject<int>();
            _onXpUpdate = new Subject<OnXpUpdateData>();
            _player = player;
            _playerSetting = playerSetting;
            _cameraProvider = cameraProvider;
            _fxViewPresenter = fxViewPresenter;
            _iconSettings = iconSettings;
        }

        public void Initialize()
        {
            _player.Xp
                .Subscribe(XpUpdate)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        private void XpUpdate(int value)
        {
            var currentLevel = _player.Level.Value;
            var levelByXp = GetLevelByXp(out var nextLevelXpBound);
            if (currentLevel > 0 && currentLevel < levelByXp)
            {
                Debug.Log($"[PlayerService] SetLevel {levelByXp}");
                _player.SetLevel(levelByXp);
                _onLevelUp.OnNext(_player.Level.Value);
            }

            _onXpUpdate.OnNext(new OnXpUpdateData(GetDifferenceXp(), nextLevelXpBound));
        }

        public int GetLevelByXp(out int nextLevelBound)
        {
            var currentXp = GetDifferenceXp();
            var currentLevel = _player.Level.Value;
            for (var index = currentLevel; index < _playerSetting.XpLevels.Count; index++)
            {
                var levelBound = _playerSetting.XpLevels[index];
                if (levelBound > currentXp)
                {
                    nextLevelBound = levelBound;
                    return index;
                }
            }

            nextLevelBound = _playerSetting.XpLevels[_playerSetting.XpLevels.Count - 1];
            return _playerSetting.XpLevels.Count;
        }

        public int GetDifferenceXp()
        {
            var currentLevel = _player.Level.Value;
            if (currentLevel > 0)
            {
                var sum = 0;
                for (var i = 0; i < currentLevel; i++)
                {
                    sum += _playerSetting.XpLevels[i];
                }

                return _player.Xp.Value - sum;
            }

            return _player.Xp.Value;
        }

        public void AddCoinsWithAnimation(int amount, Vector3 effectPosition,
            bool convertWorldToScreenPoint = true)
        {
            if (convertWorldToScreenPoint)
            {
                effectPosition = _cameraProvider.GetScreenPosition(effectPosition);
            }

            var icon = _iconSettings.Coin;
            _fxViewPresenter.ShowCollectSimpleAnimation(amount, icon, effectPosition);
            _player.SetCoins(_player.Coins.Value + amount);
        }
        
        public void AddCoinsWithAnimation(int amount, RectTransform rectSource)
        {
            var icon = _iconSettings.Coin;
            _fxViewPresenter.ShowCollectAdvanceAnimation(amount, icon, rectSource);
            _player.SetCoins(_player.Coins.Value + amount);
        }
        
        public void RemoveCoinsWithAnimation(int amount, Vector3 effectPosition,
            bool convertWorldToScreenPoint = true)
        {
            if (convertWorldToScreenPoint)
            {
                effectPosition = _cameraProvider.GetScreenPosition(effectPosition);
            }
            
            var icon = _iconSettings.Coin;
            _fxViewPresenter.ShowSpendingAnimation(amount, icon, effectPosition);
            _player.SetCoins(_player.Coins.Value - amount);
        }
    }
}