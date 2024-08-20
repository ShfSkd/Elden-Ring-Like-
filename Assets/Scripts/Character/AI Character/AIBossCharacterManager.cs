using SKD.Game_Saving;
using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int _bossID = 0;
        [SerializeField] bool _hasBeenDefeted;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                // If our Save Data does not contain information on this boss, add it now
                if (!WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.ContainsKey(_bossID))
                {
                    WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.Add(_bossID, false);
                    WorldSaveGameManager.Instance._currentCharacterData._bossesDefeated.Add(_bossID, false);
                }
                // otherwise, load the data that already exist on this boss
                else
                {
                    _hasBeenDefeted = WorldSaveGameManager.Instance._currentCharacterData._bossesDefeated[_bossID];

                    if (_hasBeenDefeted)
                    {
                        _aICharacterNetworkManager._isActive.Value = false;
                    }
                }
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                _characterNetworkManager._currentHealth.Value = 0;

                if (!manuallySelectDeathAnimation)
                    _characterAnimationManager.PlayTargetActionAnimation("Dead_01", true);

                _hasBeenDefeted = true;

                if (!WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.ContainsKey(_bossID))
                {
                    WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.Add(_bossID, true);
                    WorldSaveGameManager.Instance._currentCharacterData._bossesDefeated.Add(_bossID, true);
                }
                // otherwise, load the data that already exist on this boss
                else
                {
                    WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.Remove(_bossID);
                    WorldSaveGameManager.Instance._currentCharacterData._bossesDefeated.Remove(_bossID);

                    WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.Add(_bossID, true);
                    WorldSaveGameManager.Instance._currentCharacterData._bossesDefeated.Add(_bossID, true);
                }

                WorldSaveGameManager.Instance.SaveGame();
            }
            yield return new WaitForSeconds(5);
        }
    }
}
