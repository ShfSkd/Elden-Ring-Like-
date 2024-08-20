using SKD.Character.AI_Character;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SKD.World_Manager
{
    public class WorldAIManager : MonoBehaviour
    {
        private static WorldAIManager instance;
        public static WorldAIManager Instance { get { return instance; } }

 

        [Header("Characters")]
        [SerializeField] List<AICharacterSpawner> _aICharacterSpawnerList;
        [SerializeField] List<GameObject> _spawnInCharctersList = new List<GameObject>();

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
        private void DespawnAllCharacters()
        {
            foreach (var character in _spawnInCharctersList)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        }
        private void DisableAllCharacters()
        {

        }
    }
}