using SKD.World_Manager;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character.AI_Character
{
    public class AICharacterSpawner : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] GameObject _characterGameObject;
        [SerializeField] GameObject _instaniateGameObject;
        private void Awake()
        {
        }
        private void Start()
        {
            WorldAIManager.Instance.SpawnCharacters(this);

            gameObject.SetActive(false);
        }
        public void AttemptToSpawnCharacter()
        {
            if (_characterGameObject != null)
            {
                _instaniateGameObject = Instantiate(_characterGameObject);
                _instaniateGameObject.transform.position = transform.position;
                _instaniateGameObject.transform.rotation = transform.rotation;
                _instaniateGameObject.GetComponent<NetworkObject>().Spawn();
                WorldAIManager.Instance.AddCharacterToSpawnCharactersList(_instaniateGameObject.GetComponent<AICharacterManager>());
            }
        }
    }
}