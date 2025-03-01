namespace Libs.OpenUI.Localization
{
    public interface ILocalizationSetter
    {
        void InitLocalizableProject(UiView localizable);
        void InitLocalizable(UiView localizable);
        void Remove(UiView localizable);
        void Clear();
    }
}