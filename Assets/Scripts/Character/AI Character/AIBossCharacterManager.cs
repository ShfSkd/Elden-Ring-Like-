using SKD.Character.AI_Character.States;
using SKD.UI.DurkUI;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int _bossID = 0;

        [Header("Music")]
        [SerializeField] AudioClip _bossIntroClip;
        [SerializeField] AudioClip _bossLoopClip;

        [Header("Status")]
        public NetworkVariable<bool> _bossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> _hasBeenAwaken = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] List<FogWallIntractable> _fogWallsList;
        [SerializeField] string _sleepAnimation;
        [SerializeField] string _awakeAnimation;

        [Header("State")]
        [SerializeField] BossSleepState _bossSleepState;

        [Header("Phase Shift")]
        public float _minimumHealthPercentToShift = 50f;
        [SerializeField] string _phaseShiftAnimation = "Phase change 01";
        [SerializeField] CombatStanceState _phase02CombatStanceState;

        protected override void Awake()
        {
            base.Awake();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
            OnBossFightIsActiveChanged(false, _bossFightIsActive.Value); // If you join when the fight is already active, you will get a HP bar 

            if (IsOwner)
            {
                _bossSleepState = Instantiate(_bossSleepState);
                _currentState = _bossSleepState;
            }
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
                    _hasBeenDefeated.Value = WorldSaveGameManager.Instance._currentCharacterData._bossesDefeated[_bossID];
                    _hasBeenAwaken.Value = WorldSaveGameManager.Instance._currentCharacterData._bossesAwakened[_bossID];

                }
                // lOCATE THE FOG 
                // You can either share the same id for the fog and the boos, or simply place a fog wall Id variable here on look for it using that
                StartCoroutine(GetFogWallFromWorldObjectManager());

                if (_hasBeenAwaken.Value)
                {
                    for (int i = 0; i < _fogWallsList.Count; i++)
                    {
                        _fogWallsList[i]._isActive.Value = true;
                    }
                }
                if (_hasBeenDefeated.Value)
                {
                    for (int i = 0; i < _fogWallsList.Count; i++)
                    {
                        _fogWallsList[i]._isActive.Value = false;
                    }
                    _aICharacterNetworkManager._isActive.Value = false;
                }

            }

            if (!_hasBeenAwaken.Value)
            {
                _animator.Play(_sleepAnimation);
            }
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            _bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;
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
            PlayerUIManger.instance._playerUIPopUpManager.SendBossDefeatedPopUp("Great Enemy Felled");
            if (IsOwner)
            {
                _characterNetworkManager._currentHealth.Value = 0;
                _isDead.Value = true;

                _bossFightIsActive.Value = false;

                foreach (var fogWall in _fogWallsList)
                {
                    fogWall._isActive.Value = false; 
                }

                if (!manuallySelectDeathAnimation)
                    _characterAnimationManager.PlayTargetActionAnimation("Dead_01", true);

                _hasBeenDefeated.Value = true;

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
            if (IsOwner)
            {
                if (!_hasBeenAwaken.Value)
                {
                    _characterAnimationManager.PlayTargetActionAnimation(_awakeAnimation, true);
                }
                _bossFightIsActive.Value = true;
                _hasBeenAwaken.Value = true;
                _currentState = _idle;

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
        private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (_bossFightIsActive.Value)
            {
                WorldSoundFXManager.instance.PlayBossTrack(_bossIntroClip, _bossLoopClip);

                // Create a HP bar for each boss that is in the fight
                GameObject bossHealthBar =
                    Instantiate(PlayerUIManger.instance._playerUIHUDManager._bossHealthBarObject, PlayerUIManger.instance._playerUIHUDManager._bossHealthBarParent);

                UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();

                bossHPBar.EnableBossHPBar(this);
            }
            else
            {
                WorldSoundFXManager.instance.StopBossMusic();
            }
        }
        public void PhaseShift()
        {
            _characterAnimationManager.PlayTargetActionAnimation(_phaseShiftAnimation,true);
            _combatStance = Instantiate(_phase02CombatStanceState);
            _currentState = _combatStance;
        }

    }
}
