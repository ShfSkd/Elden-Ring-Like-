using SKD.Character.AI_Character;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SKD.World_Manager
{
    public class WorldObjectManager : MonoBehaviour
    {
        private static WorldObjectManager instance;
        public static WorldObjectManager Instance { get { return instance; } }

        [Header("Objects")]
        [SerializeField] List<NetworkObjectSpawner> _networkObjectSpawnerList;
        [SerializeField] List<GameObject> _spawnInObjects = new List<GameObject>();

        [Header("Fog Walls")]
        public List<FogWallIntractable> _fogWallsList = new List<FogWallIntractable>();

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        public void SpawnObjects(NetworkObjectSpawner objectSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                _networkObjectSpawnerList.Add(objectSpawner);
                objectSpawner.AttemptToSpawnObjects();
            }
        }

        public void AddFogWallToList(FogWallIntractable fogWall)
        {
            if (!_fogWallsList.Contains(fogWall))
            {
                _fogWallsList.Add(fogWall);
            }
        }
        public void RemoveFogWallFromList(FogWallIntractable fogWall)
        {
            if (_fogWallsList.Contains(fogWall))
            {
                _fogWallsList.Remove(fogWall);
            }
        }
    }
}