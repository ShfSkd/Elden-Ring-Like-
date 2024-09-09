using SKD.Character;
using SKD.Character.AI_Character;
using SKD.Character.Player;
using SKD.UI.PlayerUI;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SKD.UI
{
    // Perform identically to the UI_StateBar, except this bar appears and disappears in world space (will always face the camera)
    public class UI_CharacterHPBar : UI_StatBar
    {
        private CharacterManager _character;
        private AICharacterManager _aICharacter;
        private PlayerManager _playerCharacter;

        [SerializeField] bool _displayCharacterNameOnDamage;
        [SerializeField] float _defaultTimeBeforeBarHides = 3f;
        [SerializeField] float _hideTimer;
        [SerializeField] float _currentDamageTaken;
        [SerializeField] TextMeshProUGUI _characterName;
        [SerializeField] TextMeshProUGUI _charcterDamage;
        [HideInInspector] public int _oldHealthValue;

        protected override void Awake()
        {
            base.Awake();

            _character = GetComponentInParent<CharacterManager>();

            if (_character != null)
            {
                _aICharacter = _character as AICharacterManager;
                _playerCharacter = _character as PlayerManager;
            }

        }
        protected override void Start()
        {
            base.Start();

           gameObject.SetActive(false);
        }
        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);

            if (_hideTimer > 0)
                _hideTimer -= Time.deltaTime;
            else
                gameObject.SetActive(false);

        }
        private void OnDisable()
        {
            _currentDamageTaken = 0;
        }
        public override void SetStat(int newValue)
        {
            if (_displayCharacterNameOnDamage)
            {
                _characterName.enabled = true;

                if (_aICharacter != null)
                    _characterName.text = _aICharacter._characterName;

                if (_playerCharacter != null)
                    _characterName.text = _playerCharacter._playerNetworkManager._characterName.Value.ToString();

            }
            // Call this increase max health changes from a character effect/buff etc
            _slider.maxValue = _character._characterNetworkManager._maxHealth.Value;

            // Total damage taken whilst the bar is active
            _currentDamageTaken = Mathf.RoundToInt(_currentDamageTaken + (_oldHealthValue - newValue));

            if (_currentDamageTaken < 0)
            {
                _currentDamageTaken = Mathf.Abs(_currentDamageTaken);
                _charcterDamage.text = "+ " + _currentDamageTaken.ToString();
            }
            else
            {
                _charcterDamage.text = "- " + _currentDamageTaken.ToString();

            }
            _slider.value = newValue;

            if (_character._characterNetworkManager._currentHealth.Value != _character._characterNetworkManager._maxHealth.Value)
            {
                _hideTimer = _defaultTimeBeforeBarHides;
                gameObject.SetActive(true);
            }
        }
    }
}