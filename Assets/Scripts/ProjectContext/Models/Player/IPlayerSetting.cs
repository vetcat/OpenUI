using System.Collections.Generic;

namespace ProjectContext.Models.Player
{
    public interface IPlayerSetting
    {
        int MaxHealth { get; }
        List<int> XpLevels { get; }
    }
}