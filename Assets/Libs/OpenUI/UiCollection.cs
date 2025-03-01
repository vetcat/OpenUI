using System;
using System.Collections.Generic;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Libs.OpenUI
{
	public abstract class UiCollection<TView> : MonoBehaviour where TView : UiView
	{
		[InjectOptional] private ILocalizationSetter _localizationSetter;
		[InjectOptional] private SignalBus _signalBus;
		public Transform CollectionRoot => _collectionRoot;
		[SerializeField] private Transform _collectionRoot;

		[SerializeField] private TView _collectionPrefab;

		private readonly List<TView> _items = new();
		private readonly List<TView> _itemsPool = new();
		public readonly CompositeDisposable ItemsCompositeDisposable = new();

		public TView GetCollectionPrefab => _collectionPrefab;
		public IObservable<Unit> OnAddItemAddItemsAsyncComplete => _onAddItemAddItemsAsyncComplete;
		private ReactiveCommand _onAddItemAddItemsAsyncComplete = new();

		private readonly Queue<Action<TView>> _spawnQueue = new();
		private IDisposable _spawnProcess;

		public void SetLocalizationSetter(ILocalizationSetter localizationSetter)
		{
			_localizationSetter = localizationSetter;
		}

		public void InitPool(int count = 5)
		{
			for (var i = 0; i < count; i++)
			{
				var item = AddItemInternal(false);
				_itemsPool.Add(item);
			}
		}

		public bool HasSpawnProcess => _spawnProcess != null;

		public void AddItemsAsync<TItemData>(IEnumerator<TItemData> enumerator, Action<TItemData, TView> action)
		{
			if (_spawnProcess != null)
				return;

			_spawnProcess = Observable.EveryUpdate().Subscribe(_ =>
			{
				if (enumerator.MoveNext())
				{
					var item = AddItem();
					action?.Invoke(enumerator.Current, item);
				}
				else
				{
					StopSpawnProcess();
					_onAddItemAddItemsAsyncComplete.Execute();
				}
			}).AddTo(ItemsCompositeDisposable);
		}

		public void AddItemAsync(Action<TView> action)
		{
			_spawnQueue.Enqueue(action);
		
			if (_spawnProcess != null)
				return;
		
			_spawnProcess = Observable.EveryUpdate().Subscribe(_ =>
			{
				var item = AddItem();
				action?.Invoke(item);
		
				_spawnQueue.Dequeue();
		
				if (_spawnQueue.Count == 0)
				{
					StopSpawnProcess();
					_onAddItemAddItemsAsyncComplete.Execute();
				}
			}).AddTo(ItemsCompositeDisposable);
		}

		public TView AddItem()
		{
			TView resultItem = null;
			foreach (var item in _itemsPool)
			{
				if (!item.IsShow())
				{
					resultItem = item;
					resultItem.gameObject.SetActive(true);
					break;
				}
			}

			if (resultItem == null)
				resultItem = AddItemInternal();

			if (!_items.Contains(resultItem))
				_items.Add(resultItem);

			if (_itemsPool.Contains(resultItem))
				_itemsPool.Remove(resultItem);

			resultItem.transform.SetAsLastSibling();
			return resultItem;
		}

		public void RemoveItem(TView item)
		{
			if (_itemsPool.Contains(item))
				_itemsPool.Remove(item);

			if (_items.Contains(item))
				_items.Remove(item);

			Unsubscribe(item);

			item.gameObject.SetActive(false);
		}

		public List<TView> GetItems()
		{
			return _items;
		}

		public void Clear()
		{
			StopSpawnProcess();
			
			foreach (var item in _items)
			{
				if (!_itemsPool.Contains(item))
					_itemsPool.Add(item);

				Unsubscribe(item);
				item.gameObject.SetActive(false);
			}

			_items.Clear();
			ItemsCompositeDisposable.Clear();
			_onAddItemAddItemsAsyncComplete.Dispose();
			_onAddItemAddItemsAsyncComplete = new();
		}
		
		public void StopSpawnProcess()
		{
			_spawnProcess?.Dispose();
			_spawnProcess = null;
		}

		private TView AddItemInternal(bool checkActive = true)
		{
			TView item = Instantiate(_collectionPrefab, _collectionRoot);

			_localizationSetter?.InitLocalizable(item);

			if (checkActive)
			{
				if (!item.gameObject.activeInHierarchy)
					item.gameObject.SetActive(true);
			}

			if (_signalBus != null)
				item.AddSignalClick(_signalBus);

			_items.Add(item);

			return item;
		}

		private void Unsubscribe(TView item)
		{
			var trigger = item.GetComponent<ObservableDestroyTrigger>();
			if (trigger)
				trigger.Unsubscribe();
		}

		public int Count() => _items.Count;
		public int GetPoolCount() => _itemsPool.Count;
	}
}