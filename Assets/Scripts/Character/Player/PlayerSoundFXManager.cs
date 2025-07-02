using System.Collections;
using SKD.World_Manager;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        private PlayerManager _player;

        protected override void Awake()
        {
            base.Awake();
            _player = GetComponent<PlayerManager>();
        }

        public override void PlayBlockingSFX()
        {
            PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(_player._playerCombatManager._currentWeaponBeingUsed._blocking));
        }
    }
}