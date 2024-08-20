using SKD.World_Manager;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class NetworkObjectSpawner : MonoBehaviour
    {

        [Header("Object")]
        [SerializeField] GameObject _networkGameObject;
        [SerializeField] GameObject _instaniateGameObject;
        private void Awake()
        {
        }
        private void Start()
        {
            WorldObjectManager.Instance.SpawnObjects(this);
            gameObject.SetActive(false);
        }
        public void AttemptToSpawnObjects()
        {
            if (_networkGameObject != null)
            {
                _instaniateGameObject = Instantiate(_networkGameObject);
                _instaniateGameObject.transform.position = transform.position;
                _instaniateGameObject.transform.rotation = transform.rotation;
                _instaniateGameObject.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}