using SKD.Character.Player;
using UnityEngine;
using UnityEngine.Serialization;
namespace SKD.Spells.Items
{
    public class SpellItem : Item
    {
        [FormerlySerializedAs("spellClass")]
        [Header("Spell Class")]
        public SpellClass _spellClass;

        [Header("Spell Modifiers")]
        public float _fullChargeEffectMultiplier = 2f;

        [Header("Spell Slot")]
        public int _spellSlotUsed = 1;
        public int _staminaCost = 25;
        public int _focusPointsCost = 25;

        [Header("Spell Effects")]
        [SerializeField] protected GameObject _spellCastWarmUpFX;
        [SerializeField] protected GameObject _spellChargeFX;
        [SerializeField] protected GameObject _spellCastReleaseFX;
        [SerializeField] protected GameObject _spellCastReleaseFXFullCharge;

        [Header("Animations")]
        [SerializeField] protected string _mainHandSpellAnimation;
        [SerializeField] protected string _offHandSpellAnimation;

        [Header("Sound FX")]
        public AudioClip _releaseSFX;
        public AudioClip _warmUpSFX;
        // This is where you play thr "Warm up" animation
        public virtual void AttemptToCastSpell(PlayerManager player)
        {

        }
        // This is where you play the "Throw"  or "Cast" animation
        public virtual void SuccessfullyCastSpell(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player._playerNetworkManager._currentFocusPoints.Value -= _focusPointsCost;
                player._playerNetworkManager._currentStamina.Value -= _staminaCost;
            }
        }
        public virtual void SuccessfullyCastSpellFullCharge(PlayerManager player)
        {

        }
        public virtual void SuccessfullyChargeSpell(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player._playerNetworkManager._currentFocusPoints.Value -= Mathf.RoundToInt(_focusPointsCost * _fullChargeEffectMultiplier);
                player._playerNetworkManager._currentStamina.Value -= _staminaCost * _fullChargeEffectMultiplier;
            }
        }
        // Spell FX that are instantiated when spell has been successfully cast
        public virtual void InstantiateWarmUpSpellFX(PlayerManager player)
        {

        }
        // Helper function to check weather or not are we are able to to use this spell when attempting to cast 
        public virtual bool CanICastThisSpell(PlayerManager player)
        {
            if (player._playerNetworkManager._currentFocusPoints.Value <= _focusPointsCost)
                return false;

            if (player._playerNetworkManager._currentStamina.Value <= _staminaCost)
                return false;

            if (player._isPerformingAction)
                return false;

            if (player._playerNetworkManager._isJumping.Value)
                return false;

            return true;
        }

    }
}