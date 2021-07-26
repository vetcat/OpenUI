using Libs.OpenUI;
using ProjectContext.Models.Player;
using SampleScene.UiViews.Views;
using UniRx;

namespace SampleScene.UiViews.Presenters
{
    public class UiTopRightViewPresenter : UiPresenter<UiTopRightView>
    {
        private readonly IPlayerService _playerService;

        public UiTopRightViewPresenter(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public override void Initialize()
        {
            base.Initialize();
            _playerService.Player.Coins.SubscribeToText(View.TextCoinsAmount);
        }
    }
}