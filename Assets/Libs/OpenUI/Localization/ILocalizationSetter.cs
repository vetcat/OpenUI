namespace Libs.OpenUI.Localization
{
    public interface ILocalizationSetter
    {
        void InitLocalizable(ILocalizable localizable);
        void Remove(ILocalizable localizable);
        void Clear();
    }
}