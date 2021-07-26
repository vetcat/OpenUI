using DG.Tweening;
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
        private CharacterView _characterView;

        public CharacterViewController(CharacterView.Factory characterFactory)
        {
            _characterFactory = characterFactory;
        }

        public void Initialize()
        {
            _characterView = _characterFactory.Create();
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
        }
    }
}