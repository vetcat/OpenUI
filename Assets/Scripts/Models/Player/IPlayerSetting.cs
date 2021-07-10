using System.Collections.Generic;

namespace Models.Player
{
    public interface IPlayerSetting
    {
        int MaxHealth { get; }
        List<int> XpLevels { get; }
    }
}