namespace Libs.OpenUI.Localization
{
    public struct LocalizationItemWrapper
    {
        public string Key;
        public object[] Args;

        public LocalizationItemWrapper(string key, params object[] args)
        {
            Key = key;
            Args = args;
        }
    }
}