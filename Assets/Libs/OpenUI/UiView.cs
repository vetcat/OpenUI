using System;
using System.Collections.Generic;
using Libs.OpenUI.Localization;
using UniRx;
using UnityEngine;

namespace Libs.OpenUI
{
    public class UiView : MonoBehaviour
    {
        public List<LocalizationItem> LocalizationFields => localizationFields;
        [SerializeField] private List<LocalizationItem> localizationFields = new();
        
        private readonly ReactiveCommand<UiView> _onShow = new();
        public IObservable<UiView> OnShow => _onShow;

        private readonly ReactiveCommand<UiView> _onHide = new();
        public IObservable<UiView> OnHide => _onHide;


        public void Show(bool isShow = true)
        {
            gameObject.SetActive(isShow);
            
            if (isShow)
                _onShow.Execute(this);
            else
                _onHide.Execute(this);
        }

        public void Hide()
        {
            if (!IsShow())
                return;

            gameObject.SetActive(false);
            _onHide.Execute(this);
        }

        public bool IsShow()
        {
            return gameObject.activeSelf;
        }
    }
}