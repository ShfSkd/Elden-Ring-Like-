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
        private AICharacterManager _aiCharacter;
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
                _aiCharacter = _instaniateGameObject.GetComponent<AICharacterManager>();

                if (_aiCharacter != null)
                    WorldAIManager.Instance.AddCharacterToSpawnCharactersList(_aiCharacter);
            }
        }
        public void ResetCharacter()
        {
            if (_instaniateGameObject == null)
                return;

            if (_aiCharacter == null)
                return;

            _instaniateGameObject.transform.position = transform.position;
            _instaniateGameObject.transform.rotation = transform.rotation;
            _aiCharacter._aICharacterNetworkManager._currentHealth.Value = _aiCharacter._aICharacterNetworkManager._maxHealth.Value;

            if (_aiCharacter._isDead.Value)
            {
                _aiCharacter._isDead.Value = false;
                _aiCharacter._characterAnimationManager.PlayTargetActionAnimation("Empty", false, false, true, true, true, true);
            }

            _aiCharacter._characterUIManager.ResetCharacterHpBar();
        }
    }
}