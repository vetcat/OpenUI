using System.Collections.Generic;
using UnityEngine;

namespace Models.Player
{
    [CreateAssetMenu(menuName = "Settings/PlayerSetting", fileName = "PlayerSetting")]
    public class PlayerSetting : ScriptableObject, IPlayerSetting
    {
        public List<int> XpLevels => _xpLevels;
        [SerializeField] private List<int> _xpLevels;
    }
}