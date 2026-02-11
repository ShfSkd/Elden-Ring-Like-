using System;
using SKD.Character.Player;
using SKD.WorldManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SKD.UI.PlayerUI
{
    public class PlayerUILevelUpManager : PlayerUIMenu
    {
        [Header("Levels")]
        [SerializeField] int[] _playerLevels = new int [100];
        [SerializeField] int _baseLevelCost = 83;
        [SerializeField] int _totalLevelUpCost = 0;

        [Header("Character Stats")]
        [SerializeField] TextMeshProUGUI _characterLevelText;
        [SerializeField] TextMeshProUGUI _runesHeldText;
        [SerializeField] TextMeshProUGUI _runesNeddedText;
        [SerializeField] TextMeshProUGUI _vigorLevelText;
        [SerializeField] TextMeshProUGUI _mindLevelText;
        [SerializeField] TextMeshProUGUI _eduranceLevelText;
        [SerializeField] TextMeshProUGUI _strengthLevelText;
        [SerializeField] TextMeshProUGUI _dexterityLevelText;
        [SerializeField] TextMeshProUGUI _intelligenceLevelText;
        [SerializeField] TextMeshProUGUI _faithLevelText;


        [Header("Projected Character Stats")]
        [SerializeField] TextMeshProUGUI _projectedCharacterLevelText;
        [SerializeField] TextMeshProUGUI _projectedRunesHeldText;
        [SerializeField] TextMeshProUGUI _projectedVigorLevelText;
        [SerializeField] TextMeshProUGUI _projectedMindLevelText;
        [SerializeField] TextMeshProUGUI _projectedEnduranceLevelText;
        [SerializeField] TextMeshProUGUI _projectedStrengthLevelText;
        [SerializeField] TextMeshProUGUI _projectedDexterityLevelText;
        [SerializeField] TextMeshProUGUI _projectedIntelligenceLevelText;
        [SerializeField] TextMeshProUGUI _projectedFaithLevelText;

        [Header("Sliders")]
        public CharacterAttributes _currentSelectedAttribute;
        public Slider _vigorSlider;
        public Slider _mindSlider;
        public Slider _eduranceSlider;
        public Slider _strengthSlider;
        public Slider _dexteritySlider;
        public Slider _intelligenceSlider;
        public Slider _faithSlider;

        [Header("Buttons")]
        [SerializeField] Button _confirerLevelButton;
        void Awake()
        {
            SetAllLevelCost();
        }

        public override void OpenMenu()
        {
            base.OpenMenu();

            SetCurrentStats();
        }

        private void SetCurrentStats()
        {
            // Character Level
            _characterLevelText.text = PlayerUIManager.Instance._localPlayer._characterStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();
            _projectedCharacterLevelText.text = PlayerUIManager.Instance._localPlayer._characterStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();

            // Runes
            _runesHeldText.text = PlayerUIManager.Instance._localPlayer._characterStatsManager._runesDroppedOnDeath.ToString();
            _projectedRunesHeldText.text = PlayerUIManager.Instance._localPlayer._characterStatsManager._runesDroppedOnDeath.ToString();
            _runesNeddedText.text = "0";

            // Attributes
            _vigorLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._vigor.Value.ToString();
            _projectedVigorLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._vigor.Value.ToString();
            _vigorSlider.minValue = PlayerUIManager.Instance._localPlayer._playerNetworkManager._vigor.Value;

            _mindLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._mind.Value.ToString();
            _projectedMindLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._mind.Value.ToString();
            _mindSlider.minValue = PlayerUIManager.Instance._localPlayer._playerNetworkManager._mind.Value;

            _eduranceLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._endurance.Value.ToString();
            _projectedEnduranceLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._endurance.Value.ToString();
            _eduranceSlider.minValue = PlayerUIManager.Instance._localPlayer._playerNetworkManager._endurance.Value;

            _strengthLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._strength.Value.ToString();
            _projectedStrengthLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._strength.Value.ToString();
            _strengthSlider.minValue = PlayerUIManager.Instance._localPlayer._playerNetworkManager._strength.Value;


            _dexterityLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._dexterty.Value.ToString();
            _projectedDexterityLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._dexterty.Value.ToString();
            _dexteritySlider.minValue = PlayerUIManager.Instance._localPlayer._playerNetworkManager._dexterty.Value;

            _intelligenceLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._intelligence.Value.ToString();
            _projectedIntelligenceLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._intelligence.Value.ToString();
            _intelligenceSlider.minValue = PlayerUIManager.Instance._localPlayer._playerNetworkManager._intelligence.Value;

            _faithLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._faith.Value.ToString();
            _projectedFaithLevelText.text = PlayerUIManager.Instance._localPlayer._playerNetworkManager._faith.Value.ToString();
            _faithSlider.minValue = PlayerUIManager.Instance._localPlayer._playerNetworkManager._faith.Value;

            _vigorSlider.Select();
            _vigorSlider.OnSelect(null);

        }
        // This is called evey time a level slider is changed
        public void UpdateSliderBasedOnCurrentlySelectedAttributes()
        {
            PlayerManager player = PlayerUIManager.Instance._localPlayer;

            switch (_currentSelectedAttribute)
            {
                case CharacterAttributes.Vigor:
                    _projectedVigorLevelText.text = _vigorSlider.value.ToString();
                    break;
                case CharacterAttributes.Mind:
                    _projectedMindLevelText.text = _mindSlider.value.ToString();
                    break;
                case CharacterAttributes.Endurance:
                    _projectedEnduranceLevelText.text = _eduranceSlider.value.ToString();
                    break;
                case CharacterAttributes.Strength:
                    _projectedStrengthLevelText.text = _strengthSlider.value.ToString();
                    break;
                case CharacterAttributes.Dexterity:
                    _projectedDexterityLevelText.text = _dexteritySlider.value.ToString();
                    break;
                case CharacterAttributes.Intelligence:
                    _projectedIntelligenceLevelText.text = _intelligenceSlider.value.ToString();
                    break;
                case CharacterAttributes.Faith:
                    _projectedFaithLevelText.text = _faithSlider.value.ToString();
                    break;
                default:
                    break;
            }

            // Passed our current level and our projected level to set our cost for leveling up
            CalculateLevelCost(player._characterStatsManager.CalculateCharacterLevelBasedOnAttributes(),
                player._characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true));

            _projectedCharacterLevelText.text = player._characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true).ToString();
            _runesNeddedText.text = _totalLevelUpCost.ToString();

            // 1.check Cost
            if (_totalLevelUpCost > player._playerStatsManager._runes)
            {
                // 1. Disable confirm button so you cant level up 
                _confirerLevelButton.interactable = false;
                // 2. Optionally charge level up fields text to red
            }
            else
            {
                _confirerLevelButton.interactable = true;
            }

            ChangeTextColorDependingOnCost();
        }

        public void ConfirmLevels()
        {
            PlayerManager player = PlayerUIManager.Instance._localPlayer;

            // 3. Deduct cost from total runes
            player._playerStatsManager._runes -= _totalLevelUpCost;

            // 4. Set new stat

            player._playerNetworkManager._vigor.Value = Mathf.RoundToInt(_vigorSlider.value);
            player._playerNetworkManager._mind.Value = Mathf.RoundToInt(_mindSlider.value);
            player._playerNetworkManager._endurance.Value = Mathf.RoundToInt(_eduranceSlider.value);
            player._playerNetworkManager._strength.Value = Mathf.RoundToInt(_strengthSlider.value);
            player._playerNetworkManager._dexterty.Value = Mathf.RoundToInt(_dexteritySlider.value);
            player._playerNetworkManager._intelligence.Value = Mathf.RoundToInt(_intelligenceSlider.value);
            player._playerNetworkManager._faith.Value = Mathf.RoundToInt(_faithSlider.value);

            SetCurrentStats();
            ChangeTextColorDependingOnCost();
            // Saving game after stats
            WorldSaveGameManager.Instance.SaveGame();
        }

        private void SetAllLevelCost()
        {
            for (int i = 0; i < _playerLevels.Length; i++)
            {
                // Level 0 haas no cost
                if (i == 0)
                    continue;

                // This is a safeguard to stop adding the cost if the player level some how exceed the size of the array we have created  
                if (i > _playerLevels.Length)
                    continue;

                _playerLevels[i] = _baseLevelCost + (50 * i);
            }
        }
        private void CalculateLevelCost(int currentLevel, int projectedLevel)
        {
            // We dont to charge for levels we already paid for
            int totalCost = 0;
            for (int i = 0; i < projectedLevel; i++)
            {
                // Do not charge until we get past our current level
                if (i < currentLevel)
                    continue;

                totalCost += _playerLevels[i];
            }
            _totalLevelUpCost = totalCost;

            _projectedRunesHeldText.text = (PlayerUIManager.Instance._localPlayer._playerStatsManager._runes - totalCost).ToString();

            if (totalCost > PlayerUIManager.Instance._localPlayer._playerStatsManager._runes)
                _projectedRunesHeldText.color = Color.red;
            else
            {
                _projectedRunesHeldText.color = Color.white;
            }
        }

        // This will change the color of the projected level
        // Red- Cant afford, Blue- can e afford it, white- if the state is unchanged
        private void ChangeTextColorDependingOnCost()
        {
            PlayerManager player = PlayerUIManager.Instance._localPlayer;

            int projectedVigorLevel = Mathf.RoundToInt(_vigorSlider.value);
            int projectedMindLevel = Mathf.RoundToInt(_mindSlider.value);
            int projectedEnduranceLevel = Mathf.RoundToInt(_eduranceSlider.value);
            int projectedStrengthLevel = Mathf.RoundToInt(_strengthSlider.value);
            int projectedDexterityLevel = Mathf.RoundToInt(_dexteritySlider.value);
            int projectedIntelligenceLevel = Mathf.RoundToInt(_intelligenceSlider.value);
            int projectedFaithLevel = Mathf.RoundToInt(_faithSlider.value);

            ChangeTextFieldToSpecificColorBasedOnStat(player, _projectedVigorLevelText, player._playerNetworkManager._vigor.Value, projectedVigorLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, _projectedMindLevelText, player._playerNetworkManager._mind.Value, projectedMindLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, _projectedEnduranceLevelText, player._playerNetworkManager._endurance.Value, projectedEnduranceLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, _projectedStrengthLevelText, player._playerNetworkManager._strength.Value, projectedStrengthLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, _projectedDexterityLevelText, player._playerNetworkManager._dexterty.Value, projectedDexterityLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, _projectedIntelligenceLevelText, player._playerNetworkManager._intelligence.Value, projectedIntelligenceLevel);
            ChangeTextFieldToSpecificColorBasedOnStat(player, _projectedFaithLevelText, player._playerNetworkManager._faith.Value, projectedFaithLevel);

            int projectPlayerLevel = player._characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true);
            int playerLevel = player._characterStatsManager.CalculateCharacterLevelBasedOnAttributes();

            if (projectPlayerLevel == playerLevel)
            {
                _projectedCharacterLevelText.color = Color.white;
                _projectedRunesHeldText.color = Color.white;
                _runesNeddedText.color = Color.white;
            }

            // we can afford it 
            if (_totalLevelUpCost <= player._playerStatsManager._runes)
            {
                _runesNeddedText.color = Color.white;

                if (projectPlayerLevel > playerLevel)
                {
                     _projectedRunesHeldText.color = Color.red;
                    _projectedCharacterLevelText.color = Color.blue;
                }

            }
            else
            {
                _runesNeddedText.color = Color.red;

                if (projectPlayerLevel > playerLevel)
                    _projectedCharacterLevelText.color = Color.red;
            }


        }

        private void ChangeTextFieldToSpecificColorBasedOnStat(PlayerManager player, TextMeshProUGUI textField, int stat, int projectedStat)
        {
            if (projectedStat == stat)
                textField.color = Color.white; 
            
            // we can afford it 
            if (_totalLevelUpCost <= player._playerStatsManager._runes)
            {
                if (projectedStat > stat)
                {
                    textField.color = Color.blue;
                }
                // If our projected state is the same, keep the color as default 
                else
                {
                    textField.color = Color.white;
                }
            }
            // We can't!
            else
            {

                if (projectedStat > stat)
                {
                    textField.color = Color.red;
                }
                else
                {
                    textField.color = Color.white;
                }
            }
        }
    }
}