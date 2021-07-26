using System.Globalization;
using DG.Tweening;
using ProjectContext.Models.Player;
using ProjectContext.Settings;
using SampleScene.UiViews.Presenters;
using SampleScene.UiViews.Views.UiObjectIndicator;
using UniRx;
using UnityEngine;
using Zenject;

namespace SampleScene.Models.Character
{
    public class MoveAreaData
    {
        public float Max { get; }
        public float Min { get; }

        public MoveAreaData(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }

    public class CharacterViewController : IInitializable
    {
        private const float Speed = 1f;
        private readonly MoveAreaData _moveArea = new MoveAreaData(-5f, 5f);
        private readonly CharacterView.Factory _characterFactory;
        private readonly IUiObjectIndicatorViewPresenter _uiObjectIndicatorViewPresenter;
        private readonly IconSettings _iconSettings;
        private readonly IPlayerService _playerService;
        private readonly IPlayerSetting _playerSetting;
        private CharacterView _characterView;
        private UiIndicatorItemView _uiIndicatorItemView;

        public CharacterViewController(CharacterView.Factory characterFactory,
            IUiObjectIndicatorViewPresenter uiObjectIndicatorViewPresenter, IconSettings iconSettings,
            IPlayerService playerService, IPlayerSetting playerSetting)
        {
            _characterFactory = characterFactory;
            _uiObjectIndicatorViewPresenter = uiObjectIndicatorViewPresenter;
            _iconSettings = iconSettings;
            _playerService = playerService;
            _playerSetting = playerSetting;
        }

        public void Initialize()
        {
            CreateCharacter();
        }

        private void CreateCharacter()
        {
            _characterView = _characterFactory.Create();
            _uiIndicatorItemView =
                _uiObjectIndicatorViewPresenter.AddCharacterIndicator(_characterView.gameObject,
                    _iconSettings.Character);
            
            _playerService.Player.Health
                .Subscribe(HealthUpdate)
                .AddTo(_uiIndicatorItemView);

            _uiIndicatorItemView.ButtonAction.OnClickAsObservable()
                .Subscribe(_ => ButtonAction())
                .AddTo(_uiIndicatorItemView);
            
            SetTargetPosition(_characterView.GetTransform);
        }

        private void MoveTo(Transform transform, Vector3 targetPosition)
        {
            var sourcePosition = transform.position;
            var distance = Vector3.Distance(sourcePosition, targetPosition);
            var duration = distance / Speed;

            _characterView.GetTransform.DOMove(targetPosition, duration).SetEase(Ease.Linear)
                .OnComplete(() => SetTargetPosition(transform));
        }

        private void SetTargetPosition(Transform transform)
        {
            var targetPositionX = Random.Range(_moveArea.Min, _moveArea.Max);
            var targetPositionZ = Random.Range(_moveArea.Min, _moveArea.Max);
            var targetPosition = new Vector3(targetPositionX, 0f, targetPositionZ);
            MoveTo(transform, targetPosition);
            _uiIndicatorItemView.TextInfo.text = targetPosition.ToString();
        }

        private void HealthUpdate(int value)
        {
            var normalizeValue = (float) value / _playerSetting.MaxHealth;
            _uiIndicatorItemView.Slider.value = normalizeValue;
            _uiIndicatorItemView.TextSliderValue.text = value.ToString(CultureInfo.InvariantCulture);
        }

        private void ButtonAction()
        {
            CharacterReward();
        }

        private void CharacterReward()
        {
            var coinsRewardAmount = Random.Range(10, 100);
            _playerService.AddCoinsWithAnimation(coinsRewardAmount, _characterView.GetRewardPosition);
        }

        private void Pause()
        {
            if (!_characterView.IsPause)
            {
                DOTween.Kill(_characterView.GetTransform);
            }
            else
            {
                SetTargetPosition(_characterView.GetTransform);
            }
            
            _characterView.Pause(!_characterView.IsPause);
        }
    }
}