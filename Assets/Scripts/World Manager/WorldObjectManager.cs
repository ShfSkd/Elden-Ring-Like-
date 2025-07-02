using SKD.Character.AI_Character;
using System.Collections;
using System.Collections.Generic;
using SKD.Interacts;
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
        
        [Header("Sites Of Grace")]
        public List<SiteOfGraceInteractable> _sitesOfGraceList = new List<SiteOfGraceInteractable>();

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
        public void AddSiteOfGraceToList(SiteOfGraceInteractable siteOfGrace)
        {
            if (!_sitesOfGraceList.Contains(siteOfGrace))
            {
                _sitesOfGraceList.Add(siteOfGrace);
            }
        }
        public void RemoveSiteOfGraceFromList(SiteOfGraceInteractable siteOfGrace)
        {
            if (_sitesOfGraceList.Contains(siteOfGrace))
            {
                _sitesOfGraceList.Remove(siteOfGrace);
            }
        }
    }
}