using DG.Tweening;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using NSubstitute;
using NUnit.Framework;
using ProjectContext.Models.Player;
using SampleScene.Installers;
using SampleScene.UiViews.Presenters;
using SampleScene.UiViews.Views;
using Tests.Editor.SetUp;
using Zenject;
using UnityEngine;

namespace Tests.Editor.UiTests.UiTopLeft
{
    /*
    * I do not recommend writing unit tests for View or Presenters often - but there is such an opportunity
    */
    public class UiTopLeftViewTests : TestBase
    {
        //mocks 
        private readonly IPlayerService _playerService = Substitute.For<IPlayerService>();
        private readonly IUiShopViewPresenter _uiShopViewPresenter = Substitute.For<IUiShopViewPresenter>();
        private readonly ILocalizationProvider _localizationProvider = Substitute.For<ILocalizationProvider>();
        private readonly ILocalizationSetter _localizationSetter = Substitute.For<ILocalizationSetter>();

        [Inject] private IPlayer _player;
        [Inject] private UiTopLeftView _uiTopLeftView;
        [Inject] private UiTopLeftViewPresenter _uiTopLeftViewPresenter;

        protected override void Install(DiContainer container)
        {
            container.Bind<IPlayerService>().FromInstance(_playerService);
            container.Bind<IUiShopViewPresenter>().FromInstance(_uiShopViewPresenter);
            container.Bind<ILocalizationProvider>().FromInstance(_localizationProvider);
            container.Bind<ILocalizationSetter>().FromInstance(_localizationSetter);

            var playerSettings = Resources.Load("Project/Settings/PlayerSetting") as PlayerSetting;
            container.Bind<IPlayerSetting>().FromInstance(playerSettings);

            container.BindInterfacesAndSelfTo<Player>().AsSingle().NonLazy();

            var playerInContainer = container.Resolve<Player>();
            _playerService.Player.Returns(playerInContainer);

            var gameUiPrefabInstaller = Resources.Load("SampleScene/SampleSceneUiInstaller") as SampleSceneUiInstaller;
            var canvas = new GameObject("Canvas").AddComponent<Canvas>();
            
            container.BindViewPresenter<UiTopLeftView, UiTopLeftViewPresenter>(
                gameUiPrefabInstaller.UiTopLeftView, canvas);
        }

        public override void SetUp()
        {
            base.SetUp();
            //returns real class Player from mock
            _playerService.Player.Returns(_player);
        }

        [Test]
        public void View_Start_IsShow()
        {
            Assert.AreEqual(true, _uiTopLeftView.IsShow());
        }

        [Test]
        public void View_Start_SetHealthFromModel()
        {
            var strHealth = _uiTopLeftView.HealthData.TextValue.text;
            var healthValueFromUi = int.Parse(strHealth);
            Assert.AreEqual(_player.Health.Value, healthValueFromUi);
        }

        [Test]
        public void View_HealthChange_ViewTextEqual()
        {
            var rndHealth = Random.Range(0, 101);
            _player.SetHealth(rndHealth);

            var strHealth = _uiTopLeftView.HealthData.TextValue.text;
            var healthValueFromUi = int.Parse(strHealth);
            Assert.AreEqual(rndHealth, healthValueFromUi);
        }
        
        //.....
        //by analogy, you can write tests for all fields in View if necessary 
        
        [Test]
        public void View_HideWithAnimation_ViewGameObjectInactive()
        {
            _uiTopLeftViewPresenter.HideWithAnimation();
            //need to forcibly complete all animations of the DOTween
            DOTween.CompleteAll();
            Assert.AreEqual(false, _uiTopLeftViewPresenter.View.gameObject.activeSelf);
        }
    }
}