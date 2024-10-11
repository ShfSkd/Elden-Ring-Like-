using SKD.Character.AI_Character;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace SKD.World_Manager
{
    public class WorldAIManager : MonoBehaviour
    {
        private static WorldAIManager instance;
        public static WorldAIManager Instance { get { return instance; } }

        [Header("Characters")]
        [SerializeField] List<AICharacterSpawner> _aICharacterSpawnerList;
        [SerializeField] List<AICharacterManager> _spawnInCharctersList;

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
        public void ResetAllCharacters()
        {
            DespawnAllCharacters();

            foreach (var spawner in _aICharacterSpawnerList)
            {
                spawner.AttemptToSpawnCharacter();
            }
        }
        private void DespawnAllCharacters()
        {
            foreach (var character in _spawnInCharctersList)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
            _spawnInCharctersList.Clear();
        }
        private void DisableAllCharacters()
        {

        }
    }
}