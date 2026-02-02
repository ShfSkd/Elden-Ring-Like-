using SKD.Character.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SKD.UI.PlayerUI
{
    public class PlayerUILevelUpManager : PlayerUIMenu
    {
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
        [SerializeField] Slider _vigorSlider;
        [SerializeField] Slider _mindSlider;
        [SerializeField] Slider _eduranceSlider;
        [SerializeField] Slider _strengthSlider;
        [SerializeField] Slider _dexteritySlider;
        [SerializeField] Slider _intelligenceSlider;
        [SerializeField] Slider _faithSlider;

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

        public void UpdateSliderBasedOnCurrentlySelectedAttributes()
        {
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

        }

        public void ConfirmLevels()
        {
            // 1. Calculate Cost

            // 2. Change stat texts or colors depending if the player can afford it or not, and if levels are higer

            // 3. Deduct cost from total runes

            // 4. Set new stat
            PlayerManager player = PlayerUIManager.Instance._localPlayer;

            player._playerNetworkManager._vigor.Value = Mathf.RoundToInt(_vigorSlider.value);
            player._playerNetworkManager._mind.Value = Mathf.RoundToInt(_mindSlider.value);
            player._playerNetworkManager._endurance.Value = Mathf.RoundToInt(_eduranceSlider.value);
            player._playerNetworkManager._strength.Value = Mathf.RoundToInt(_strengthSlider.value);
            player._playerNetworkManager._dexterty.Value = Mathf.RoundToInt(_dexteritySlider.value);
            player._playerNetworkManager._intelligence.Value = Mathf.RoundToInt(_intelligenceSlider.value);
            player._playerNetworkManager._faith.Value = Mathf.RoundToInt(_faithSlider.value);
            
            SetCurrentStats();
        }
    }
}