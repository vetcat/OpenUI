using UnityEngine;
using Zenject;

namespace SampleScene.Models.Character
{
    public class CharacterView : MonoBehaviour
    {
        public bool IsPause { get; private set; }
        [SerializeField]private Transform View;
        private Transform _transform;

        public Transform GetTransform
        {
            get
            {
                if (!_transform)
                    _transform = GetComponent<Transform>();

                return _transform;
            }
        }

        public class Factory : PlaceholderFactory<CharacterView>
        {
        }

        public void Pause(bool value)
        {
            IsPause = value;
        }
    }
}
