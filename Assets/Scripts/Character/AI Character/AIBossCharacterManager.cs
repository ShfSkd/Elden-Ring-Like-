using SKD.Game_Saving;
using SKD.World_Manager;
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
        [SerializeField] bool _hasBeenAwaken;
        [SerializeField] List<FogWallIntractable> _fogWallsList;

        [Header("Debug")]
        [SerializeField] bool _wakeUpBoss;

        protected override void Update()
        {
            base.Update();

            if (_wakeUpBoss)
            {
                _wakeUpBoss = false;
                WakeBoss();
            }
        }
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
                    _hasBeenAwaken = WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened[_bossID];

                }
                // lOCATE THE FOG 
                // You can either share the same id for the fog and the boos, or simply place a fog wall Id variable here on look for it using that
                StartCoroutine(GetFogWallFromWorldObjectManager());

                if (_hasBeenAwaken)
                {
                    for (int i = 0; i < _fogWallsList.Count; i++)
                    {
                        _fogWallsList[i]._isActive.Value = true;
                    }
                }
                if (_hasBeenDefeted)
                {
                    for (int i = 0; i < _fogWallsList.Count; i++)
                    {
                        _fogWallsList[i]._isActive.Value = false;
                    }
                    _aICharacterNetworkManager._isActive.Value = false;
                }

            }
        }
        public IEnumerator GetFogWallFromWorldObjectManager()
        {
            while (WorldObjectManager.Instance._fogWallsList.Count == 0)
                yield return new WaitForEndOfFrame();

            _fogWallsList = new List<FogWallIntractable>();
                
            foreach (var fogwWall in WorldObjectManager.Instance._fogWallsList)
            {
                if (fogwWall._fogWallID == _bossID)
                    _fogWallsList.Add(fogwWall);
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

        public void WakeBoss()
        {
            _hasBeenAwaken = true;

            if (!WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.ContainsKey(_bossID))
            {
                WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.Add(_bossID, true);
            }
            // otherwise, load the data that already exist on this boss
            else
            {
                WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.Remove(_bossID);
                WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened.Add(_bossID, true);
            }
            for (int i = 0; i < _fogWallsList.Count; i++)
            {
                _fogWallsList[i]._isActive.Value = true;
            }
        }

    }
}
