using UnityEngine;
using Zenject;

namespace SampleScene.Models.Character
{
    public class CharacterView : MonoBehaviour
    {
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
    }
}
