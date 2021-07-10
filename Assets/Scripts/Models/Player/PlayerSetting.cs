using System.Collections.Generic;
using UnityEngine;

namespace Models.Player
{
    [CreateAssetMenu(menuName = "Settings/PlayerSetting", fileName = "PlayerSetting")]
    public class PlayerSetting : ScriptableObject, IPlayerSetting
    {
        public int MaxHealth => _maxHealth;
        [SerializeField] private int _maxHealth = 100;
        public List<int> XpLevels => _xpLevels;
        [SerializeField] private List<int> _xpLevels;
    }
}