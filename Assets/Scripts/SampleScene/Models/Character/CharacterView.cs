using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace SampleScene.Models.Character
{
    public class CharacterView : MonoBehaviour
    {
        public IObservable<CharacterView> OnClick => _onClick;
        private Subject<CharacterView> _onClick;
        public bool IsPause { get; private set; }
        [SerializeField]private Transform View;
        private Transform _transform;
        private const float RewardOffset = 2f;
        
        public Transform GetTransform
        {
            get
            {
                if (!_transform)
                    _transform = GetComponent<Transform>();

                return _transform;
            }
        }
        
        private void Awake()
        {
            _onClick = new Subject<CharacterView>();
        }
        
        private void OnMouseDown()
        {
            _onClick.OnNext(this);
        }

        public Vector3 GetRewardPosition => GetTransform.position + Vector3.up * RewardOffset;

        public class Factory : PlaceholderFactory<CharacterView>
        {
        }

        public void Pause(bool value)
        {
            IsPause = value;
        }
    }
}
