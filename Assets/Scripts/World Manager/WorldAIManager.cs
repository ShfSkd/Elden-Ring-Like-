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

        [Header("Debug")]
        [SerializeField] bool _despawnCharacters;
        [SerializeField] bool _respawnCharacters;

        [Header("Characters")]
        [SerializeField] GameObject[] _aICharacters;
        [SerializeField] List<GameObject> _spawnInCharctersList = new List<GameObject>();

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                // Spawn all AI in the scene
                StartCoroutine(WaitForSceneToLoadTheSpawnCharacters());
            }
        }
        private void Update()
        {
            if (_respawnCharacters)
            {
                _respawnCharacters = false;
                SpawnAllCharacters();
            }

            if (_despawnCharacters)
            {
                _despawnCharacters = false;
                DespawnAllCharacters();
            }
        }
        private IEnumerator WaitForSceneToLoadTheSpawnCharacters()
        {
            while (!SceneManager.GetActiveScene().isLoaded)
            {
                yield return null;
            }
            SpawnAllCharacters();
        }
        private void SpawnAllCharacters()
        {
            foreach (var character in _aICharacters)
            {
                GameObject instatiateCharacter = Instantiate(character);
                instatiateCharacter.GetComponent<NetworkObject>().Spawn();
                _spawnInCharctersList.Add(instatiateCharacter);
            }
        }
        private void DespawnAllCharacters()
        {
            foreach(var character in _spawnInCharctersList)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        }
        private void DisableAllCharacters()
        {

        }
    }
}