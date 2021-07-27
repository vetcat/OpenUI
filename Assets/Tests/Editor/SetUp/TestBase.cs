using System;
using System.Collections.Generic;
using NUnit.Framework;
using UniRx;
using Zenject;

namespace Tests.Editor.SetUp
{
    public abstract class TestBase
    {
        protected CompositeDisposable Disposable => _commonDisposable;
        [Inject] private List<IInitializable> _initializables;
        [Inject] private List<IDisposable> _disposables;
        private DiContainer _container;
        protected bool AutoInitialize = true;
        private bool _isInitialized;
        private CompositeDisposable _commonDisposable;

        [SetUp]
        public virtual void SetUp()
        {
            _container = new DiContainer();
            SignalBusInstaller.Install(_container);

            Install(_container);
            _container.Inject(this);
            _container.ResolveRoots();

            if (AutoInitialize)
                Initialize();

            _commonDisposable = new CompositeDisposable();
        }

        protected void Initialize()
        {
            if (_isInitialized || _initializables == null)
                return;
            foreach (var initialized in _initializables)
                initialized.Initialize();
            _isInitialized = true;
        }

        protected abstract void Install(DiContainer container);

        [TearDown]
        public virtual void TearDown()
        {
            _isInitialized = false;

            if (_disposables != null)
            {
                foreach (var disposable in _disposables)
                    disposable.Dispose();
            }

            _container.UnbindAll();
            _commonDisposable?.Dispose();
        }
    }
}