using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager _playerManager;
        protected override void Awake()
        {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();
        }
        protected override void Start()
        {
            base.Start();

            // Why calculate it here? When we make a character creation menu, and set the stats depending on the class , this will be calculate there. Until then however, stats are never calculate, so we do it here on start. If a save file exist they will be over written when loading into a scene 
            CalculateHealthBasedOnVitalityLevel(_playerManager._playerNetworkManager._vitality.Value);
            CalculateStaminaBasedOnEnduraceLevel(_playerManager._playerNetworkManager._endurance.Value);

        }
    }
}