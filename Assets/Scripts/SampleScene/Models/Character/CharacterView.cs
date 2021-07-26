using UnityEngine;
using Zenject;

namespace SampleScene.Models.Character
{
    public class CharacterView : MonoBehaviour
    {
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
