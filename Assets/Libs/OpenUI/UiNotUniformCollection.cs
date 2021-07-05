using System.Collections.Generic;
using UnityEngine;

namespace Libs.OpenUI
{
    public class UiNotUniformCollection : MonoBehaviour
    {
        [SerializeField]
        private Transform _collectionRoot;
        
        private readonly List<UiView> _items = new List<UiView>();

        public TView AddItem<TView>(TView prefab) where TView : UiView
        {
            var item = Instantiate(prefab, _collectionRoot);

            if (!item.gameObject.activeInHierarchy)
                item.gameObject.SetActive(true);

            _items.Add(item);
            return item;
        }

        public void RemoveItem<TView>(TView item) where TView : UiView
        {
            _items.Remove(item);
            Destroy(item.gameObject);
        }

        public List<UiView> GetItems()
        {
            return _items;
        }

        public void Clear()
        {
            foreach (var item in _items)
                Destroy(item.gameObject);

            _items.Clear();
        }

        public int Count()
        {
            return _items.Count;
        }
    }
}