using System.Collections.Generic;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using UnityEngine;
using Zenject;

namespace Libs.OpenUI
{
    public abstract class UiCollection<TView> : MonoBehaviour where TView: UiView
    {
        [Inject] private ILocalizationSetter _localizationSetter;
        [Inject] private SignalBus _signalBus;
        [SerializeField]
        private Transform _collectionRoot;

        [SerializeField]
        private TView _collectionPrefab;

        private readonly List<TView> _items = new List<TView>();

        public TView GetCollectionPrefab => _collectionPrefab;

        public TView AddItem()
        {
            TView item = Instantiate(_collectionPrefab, _collectionRoot);

            if (item is ILocalizable localizable)
            {
                _localizationSetter.InitLocalizable(localizable);
            }

            if (!item.gameObject.activeInHierarchy)
                item.gameObject.SetActive(true);

            item.AddSignalClick(_signalBus);

            _items.Add(item);
            
            
            return item;
        }

        public void RemoveItem(TView item)
        {
            RemoveLocalize(item);
            _items.Remove(item);
            Destroy(item.gameObject);
        }

        private void RemoveLocalize(TView item)
        {
            if (item is ILocalizable localizable)
            {
                _localizationSetter.Remove(localizable);
            }
        }

        public List<TView> GetItems()
        {
            return _items;
        }

        public void Clear()
        {
            foreach (var item in _items)
            {
                RemoveLocalize(item);
                Destroy(item.gameObject);
            }
            _items.Clear();
        }

        public int Count()
        {
            return _items.Count;
        }
    }
}
