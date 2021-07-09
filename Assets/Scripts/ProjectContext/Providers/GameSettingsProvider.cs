using UnityEngine;
using Utils;

namespace ProjectContext.Providers
{
    public class GameSettingsProvider : IGameSettingsProvider
    {
        public float MusicVolume
        {
            get =>
                PlayerPrefs.HasKey(PlayerPrefsValues.MusicVolume)
                    ? PlayerPrefs.GetFloat(PlayerPrefsValues.MusicVolume)
                    : 1;
            set => PlayerPrefs.SetFloat(PlayerPrefsValues.MusicVolume, value);
        }
        
        public float SoundVolume
        {
            get =>
                PlayerPrefs.HasKey(PlayerPrefsValues.SoundVolume)
                    ? PlayerPrefs.GetFloat(PlayerPrefsValues.SoundVolume)
                    : 1;
            set => PlayerPrefs.SetFloat(PlayerPrefsValues.SoundVolume, value);
        }
    }
}
