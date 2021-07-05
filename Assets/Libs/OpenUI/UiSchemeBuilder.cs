using System.Collections.Generic;
using Zenject;

namespace Libs.OpenUI
{
    public abstract class UiSchemeBuilder : IInitializable
    {
        private readonly List<IUiController> _controllers = new List<IUiController>();
        private Dictionary<IUiController, int> _siblingIndexes = new Dictionary<IUiController, int>();
        [Inject] private DiContainer _container;

        public virtual void Initialize()
        {
        }

        protected void AddController<TController>() where TController : IUiController
        {
            var controller = _container.Resolve<TController>();
            _controllers.Add(controller);
        }

        public List<IUiController> GetControllers()
        {
            return _controllers;
        }

        public void Show(bool withAnimation = false)
        {
            foreach (var controller in _controllers)
            {
                if (withAnimation)
                    controller.ShowWithAnimation();
                else
                    controller.Show();
            }
        }

        public void Hide(bool withAnimation = false)
        {
            foreach (var controller in _controllers)
            {
                if (withAnimation)
                    controller.HideWithAnimation();
                else
                    controller.Hide();
            }
        }

        public void Lock()
        {
            foreach (var controller in _controllers)
                controller.Lock(true);
        }

        public void UnLock()
        {
            foreach (var controller in _controllers)
                controller.Lock(false);
        }

        public void UnlockController(UiController unlockController)
        {
            var targetController = _controllers.Find(f => f == unlockController);
            targetController?.Lock(false);
        }
        
        public void LockController(UiController unlockController)
        {
            var targetController = _controllers.Find(f => f == unlockController);
            targetController?.Lock(true);
        }
        
        public void SetAsLastSibling()
        {
            foreach (var controller in _controllers)
            {
                _siblingIndexes[controller] = controller.GetUiView.transform.GetSiblingIndex();
                controller.GetUiView.transform.SetAsLastSibling();
            }
        }
        
        public void SetAsFirstSibling()
        {
            foreach (var controller in _controllers)
            {
                _siblingIndexes[controller] = controller.GetUiView.transform.GetSiblingIndex();
                controller.GetUiView.transform.SetAsFirstSibling();
            }
        }

        public void RestoreSibling()
        {
            foreach (var controller in _controllers)
            {
                if (_siblingIndexes.ContainsKey(controller))
                {
                    var index = _siblingIndexes[controller];
                    controller.GetUiView.transform.SetSiblingIndex(index);
                }
            }
        }
    }
}