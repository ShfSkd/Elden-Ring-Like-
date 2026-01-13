using SKD.Character.AI_Character;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace SKD.World_Manager
{
    public class WorldAIManager : MonoBehaviour
    {
        private static WorldAIManager instance;
        public static WorldAIManager Instance { get { return instance; } }

        [Header("Loading")]
        public bool _isPerformingLoadingOpartion;
        
        [Header("Characters")]
        [SerializeField] List<AICharacterSpawner> _aICharacterSpawnerList;
        [SerializeField] List<AICharacterManager> _spawnInCharctersList;
        private Coroutine _spawnAllCharactersCoroutine;
        private Coroutine _despawnAllCharactersCoroutine;
        private Coroutine _resetAllCharactersCoroutine;

        [Header("Bosses")]
        [SerializeField] List<AIBossCharacterManager> _spawnInBossesList;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        public void SpawnCharacters(AICharacterSpawner aICharacterSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                _aICharacterSpawnerList.Add(aICharacterSpawner);
                aICharacterSpawner.AttemptToSpawnCharacter();
            }
        }
        public void AddCharacterToSpawnCharactersList(AICharacterManager aICharacterManager)
        {
            if (_spawnInCharctersList.Contains(aICharacterManager))
                return;

            _spawnInCharctersList.Add(aICharacterManager);

            AIBossCharacterManager bossCharacter = aICharacterManager as AIBossCharacterManager;

            if(bossCharacter != null)
            {
                if (_spawnInBossesList.Contains(bossCharacter))
                    return;

                _spawnInBossesList.Add(bossCharacter);
            }
        }
        public AIBossCharacterManager GetBossCharacterByID(int id)
        {
            return _spawnInBossesList.FirstOrDefault(boss=>boss._bossID == id);
        }
        public void SpawnAllCharacters()
        {
            _isPerformingLoadingOpartion = true;
            
            if(_spawnAllCharactersCoroutine != null)
                StopCoroutine(_spawnAllCharactersCoroutine);
            
            _spawnAllCharactersCoroutine = StartCoroutine(SpawnAllCharactersCoroutine());
          
        }
        private IEnumerator SpawnAllCharactersCoroutine()
        {
            foreach (var ai in _aICharacterSpawnerList)
            {
                yield return new WaitForFixedUpdate();
                ai.AttemptToSpawnCharacter();

                yield return null;
            }
            _isPerformingLoadingOpartion = false;
            
            yield return null;
        }
        public void ResetAllCharacters()
        {
            _isPerformingLoadingOpartion = true;
            
            if(_resetAllCharactersCoroutine != null)
                StopCoroutine(_resetAllCharactersCoroutine);
            
            _resetAllCharactersCoroutine = StartCoroutine(ResetAllCharactersCoroutine());
        }
        private IEnumerator ResetAllCharactersCoroutine()
        {
            for (int i = 0; i < _aICharacterSpawnerList.Count; i++)
            {
                yield return new WaitForFixedUpdate();
                
                _aICharacterSpawnerList[i].ResetCharacter();
                
                yield return null;
            }
            _isPerformingLoadingOpartion = false;
            
            yield return null;
        }

        private void DespawnAllCharacters()
        {
            _isPerformingLoadingOpartion = true;
            
            if(_despawnAllCharactersCoroutine != null)
                StopCoroutine(_despawnAllCharactersCoroutine);
            
            _despawnAllCharactersCoroutine = StartCoroutine(DespawnAllCharactersCoroutine());
        }
        private IEnumerator DespawnAllCharactersCoroutine()
        {
            foreach (var ai in _spawnInCharctersList)
            {
                yield return new WaitForFixedUpdate();
                
                ai.GetComponent<NetworkObject>().Despawn();
                
                yield return null;
            }
            _spawnInCharctersList.Clear();
            _isPerformingLoadingOpartion = false;
            
            yield return null;
        }
        private void DisableAllCharacters()
        {

        }
    }
}