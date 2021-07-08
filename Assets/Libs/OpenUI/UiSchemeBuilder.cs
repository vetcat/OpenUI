using System.Collections.Generic;
using Zenject;

namespace Libs.OpenUI
{
    public abstract class UiSchemeBuilder : IInitializable
    {
        private readonly List<IUiPresenter> _presenters = new List<IUiPresenter>();
        private Dictionary<IUiPresenter, int> _siblingIndexes = new Dictionary<IUiPresenter, int>();
        [Inject] private DiContainer _container;

        public virtual void Initialize()
        {
        }

        protected void AddPresenter<TPresenter>() where TPresenter : IUiPresenter
        {
            var controller = _container.Resolve<TPresenter>();
            _presenters.Add(controller);
        }

        public List<IUiPresenter> GetPresenters()
        {
            return _presenters;
        }

        public void Show(bool withAnimation = false)
        {
            foreach (var presenter in _presenters)
            {
                if (withAnimation)
                    presenter.ShowWithAnimation();
                else
                    presenter.Show();
            }
        }

        public void Hide(bool withAnimation = false)
        {
            foreach (var presenter in _presenters)
            {
                if (withAnimation)
                    presenter.HideWithAnimation();
                else
                    presenter.Hide();
            }
        }

        public void Lock()
        {
            foreach (var presenter in _presenters)
                presenter.Lock(true);
        }

        public void UnLock()
        {
            foreach (var presenter in _presenters)
                presenter.Lock(false);
        }

        public void UnlockPresenter(UiPresenter unlockPresenter)
        {
            var presenter = _presenters.Find(f => f == unlockPresenter);
            presenter?.Lock(false);
        }
        
        public void LockPresenter(UiPresenter unlockPresenter)
        {
            var presenter = _presenters.Find(f => f == unlockPresenter);
            presenter?.Lock(true);
        }
        
        public void SetAsLastSibling()
        {
            foreach (var presenter in _presenters)
            {
                _siblingIndexes[presenter] = presenter.GetUiView.transform.GetSiblingIndex();
                presenter.GetUiView.transform.SetAsLastSibling();
            }
        }
        
        public void SetAsFirstSibling()
        {
            foreach (var presenter in _presenters)
            {
                _siblingIndexes[presenter] = presenter.GetUiView.transform.GetSiblingIndex();
                presenter.GetUiView.transform.SetAsFirstSibling();
            }
        }

        public void RestoreSibling()
        {
            foreach (var presenter in _presenters)
            {
                if (_siblingIndexes.ContainsKey(presenter))
                {
                    var index = _siblingIndexes[presenter];
                    presenter.GetUiView.transform.SetSiblingIndex(index);
                }
            }
        }
    }
}