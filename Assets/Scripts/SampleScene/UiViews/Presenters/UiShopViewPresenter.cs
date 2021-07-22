using System;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.ModalWindows;
using Libs.OpenUI.UiEffects;
using ProjectContext.Settings;
using SampleScene.Models.Shop;
using SampleScene.Models.Shop.Settings;
using SampleScene.UiViews.Views.UiItemsShop;
using UniRx;
using UniRx.Triggers;

namespace SampleScene.UiViews.Presenters
{
    public class UiShopViewPresenter : UiPresenter<UiShopView>, IUiShopViewPresenter
    {
        private readonly IItemsShopSettings _itemsShopSettings;
        private readonly IconSettings _iconSettings;
        private readonly IModalWindowController _modalWindowController;
        private readonly ILocalizationProvider _localizationProvider;
        private EItemShopGroup _lastActiveGroup = EItemShopGroup.Group_1;

        public UiShopViewPresenter(IItemsShopSettings itemsShopSettings, IconSettings iconSettings,
            IModalWindowController modalWindowController, ILocalizationProvider localizationProvider)
        {
            _itemsShopSettings = itemsShopSettings;
            _iconSettings = iconSettings;
            _modalWindowController = modalWindowController;
            _localizationProvider = localizationProvider;
        }

        public override void Initialize()
        {
            base.Initialize();
            Hide();

            _localizationProvider.OnChangeLanguage
                .Subscribe(_ => ChangeLanguage())
                .AddTo(Disposables);

            View.ButtonClose.OnClickAsObservable()
                .Subscribe(_ => HideWithAnimation())
                .AddTo(Disposables);

            InitToggleGroups();
        }

        public override void ShowWithAnimation(Action complete = null)
        {
            Show();
            View.ExpandAnimation(View.Body, EAnimationTarget.Right, complete);
        }

        public override void HideWithAnimation(Action complete = null)
        {
            View.CollapseAnimation(View.Body, EAnimationTarget.Left, () =>
            {
                Hide();
                complete?.Invoke();
            });
        }

        public void Open()
        {
            ShowWithAnimation();

            ActivateToggle(_lastActiveGroup);
        }

        public void Close()
        {
            HideWithAnimation();
        }

        private void ChangeLanguage()
        {
            if (!IsShow())
                return;

            ActivateToggle(_lastActiveGroup);
        }

        private void InitToggleGroups()
        {
            var groups = Enum.GetValues(typeof(EItemShopGroup));
            foreach (EItemShopGroup group in groups)
            {
                if (group == EItemShopGroup.None)
                    continue;

                AddItemGroup(group);
            }
        }

        private void ActivateToggle(EItemShopGroup group)
        {
            var itemToggle = View.CollectionShopGroupItem.GetItems().Find(f => f.Group == group);
            itemToggle.Toggle.isOn = true;
            ShowItemsByGroup(group);
        }

        private void ShowItemsByGroup(EItemShopGroup group)
        {
            View.TextItemsType.text = Translate(group.ToString());
            View.CollectionShopItem.Clear();

            var itemsData = _itemsShopSettings.GetItemsByGroup(group);

            foreach (var itemData in itemsData)
            {
                AddShopItem(itemData);
            }

            View.ScrollRect.verticalNormalizedPosition = 1f;

            _lastActiveGroup = group;
        }

        private void AddItemGroup(EItemShopGroup group)
        {
            var item = View.CollectionShopGroupItem.AddItem();
            item.Toggle.group = View.ToggleGroup;
            item.Group = group;
            item.ImageIcon.sprite = _iconSettings.GetItemShopGroupIcon(group);

            item.Toggle.OnSelectAsObservable()
                .Subscribe(_ => ShowItemsByGroup(group))
                .AddTo(item);

            item.Toggle.OnValueChangedAsObservable()
                .Subscribe(_ => ToggleValueChanged(item))
                .AddTo(item);
        }

        private void ToggleValueChanged(ShopGroupItemView item)
        {
            var isOn = item.Toggle.isOn;
            item.ImageGlow.gameObject.SetActive(isOn);
        }

        private void AddShopItem(ItemShopVo itemData)
        {
            var item = View.CollectionShopItem.AddItem();
            item.Type = itemData.Type;
            item.ImageIcon.sprite = itemData.Icon;
            item.TextName.text = Translate(itemData.Type.ToString());
            item.TextAmount.text = itemData.Amount.ToString();
            item.EnableButtonEffects();

            item.ButtonItem.OnClickAsObservable()
                .Subscribe(_ => ButtonItemClick(item))
                .AddTo(item);
        }

        private void ButtonItemClick(ShopItemView item)
        {
            var caption = Translate("ChoiceItemCaption");
            var itemCaption = Translate(item.Type.ToString());
            var description = Translate("ChoiceItemDescription", itemCaption);
            _modalWindowController.InfoOk(caption, description);
        }
    }
}