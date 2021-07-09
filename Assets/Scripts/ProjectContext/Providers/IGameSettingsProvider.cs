namespace ProjectContext.Providers
{
    public interface IGameSettingsProvider
    {
        float MusicVolume { get; set; }
        float SoundVolume { get; set; }
    }
}
