using NUnit.Framework;
using ProjectContext.Models.Player;
using Tests.Editor.SetUp;
using UniRx;
using UnityEngine;
using Zenject;

namespace Tests.Editor.PlayerModelTests
{
    public class PlayerModelTests : TestBase
    {
        [Inject] private IPlayer _player;
        [Inject] private IPlayerSetting _playerSetting;

        protected override void Install(DiContainer container)
        {
            var playerSettings = Resources.Load("Project/Settings/PlayerSetting") as PlayerSetting;
            container.Bind<IPlayerSetting>().FromInstance(playerSettings);
            container.BindInterfacesAndSelfTo<Player>().AsSingle().NonLazy();
        }

        [Test]
        public void Player_Health_EqualValueSetting()
        {
            Assert.IsNotNull(_player);
            Assert.AreEqual(_playerSetting.MaxHealth, _player.Health.Value);
        }

        [Test]
        public void Player_Health_ChangeAtSet()
        {
            var newHealthValue = 50;
            _player.SetHealth(newHealthValue);

            Assert.AreEqual(newHealthValue, _player.Health.Value);
        }

        [Test]
        public void Player_Health_ChangeAtSetReactive()
        {
            var value = 0;

            Assert.AreNotEqual(_player.Health.Value, value);
            
            _player.Health.Subscribe(newValue => { value = newValue; })
                .AddTo(Disposable);
            
            var newHealthValue = 50;
            _player.SetHealth(newHealthValue);
            
            Assert.AreEqual(newHealthValue, value);
        }

        [Test]
        public void Player_Health_UpdateReactiveCommand()
        {
            var hasChange = false;
            _player.UpdatePlayerReactiveCommand.Subscribe(_ => hasChange = true)
                .AddTo(Disposable);

            var newHealthValue = 50;
            _player.SetHealth(newHealthValue);

            Assert.IsTrue(hasChange);
        }
    }
}