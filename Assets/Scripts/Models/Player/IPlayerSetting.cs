using System.Collections.Generic;

namespace Models.Player
{
    public interface IPlayerSetting
    {
        List<int> XpLevels { get; }
    }
}