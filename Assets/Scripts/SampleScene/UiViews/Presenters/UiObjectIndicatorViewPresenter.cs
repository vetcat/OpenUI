using System.Collections.Generic;
using Libs.OpenUI;
using SampleScene.Providers;
using SampleScene.UiViews.Views.UiObjectIndicator;
using UnityEngine;
using Zenject;

namespace SampleScene.UiViews.Presenters
{
    public class UiObjectIndicatorViewPresenter : UiPresenter<UiObjectIndicatorView>, IUiObjectIndicatorViewPresenter, ILateTickable
    {
        private const float IconOffsetY = 2f;
        private readonly ICameraProvider _cameraProvider;

        private readonly Dictionary<GameObject, UiIndicatorItemView> _objectItemViews =
            new Dictionary<GameObject, UiIndicatorItemView>();

        public UiObjectIndicatorViewPresenter(ICameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }

        public UiIndicatorItemView AddCharacterIndicator(GameObject gameObject, Sprite icon)
        {
            if (_objectItemViews.ContainsKey(gameObject))
                return _objectItemViews[gameObject];

            var item = View.CollectionIndicatorItemView.AddItem();
            item.ImageIcon.sprite = icon;

            _objectItemViews.Add(gameObject, item);

            return item;
        }

        public void RemoveCharacterIndicator(GameObject gameObject)
        {
            if (!_objectItemViews.ContainsKey(gameObject))
                return;

            _objectItemViews.Remove(gameObject);
        }
        
        public void ShowCharacterIndicator(GameObject gameObject, bool show)
        {
            if (!_objectItemViews.ContainsKey(gameObject))
                return;

            _objectItemViews[gameObject].gameObject.SetActive(show);
        }

        public void LateTick()
        {
            foreach (var objectItemView in _objectItemViews)
                ApplyPosition(objectItemView.Key, objectItemView.Value);
        }
        
        private void ApplyPosition(GameObject gameObject, UiView item, float scale = 1f)
        {
            if (!gameObject)
                return;
            
            if (!item.gameObject.activeSelf)
                return;

            item.transform.localScale = Vector3.one * scale;
            item.transform.position = GetIconScreenPosition(gameObject);
        }
        
        private Vector3 GetIconScreenPosition(GameObject gameObject)
        {
            var wordPosition = gameObject.transform.position;
            var directionPoint = wordPosition + new Vector3(0f, IconOffsetY, 0f);
        
            return _cameraProvider.GetScreenPosition(directionPoint);
        }
    }
}