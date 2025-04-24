using SKD.Character.Player;
using UnityEngine;
namespace SKD.Spells.Items
{
    [CreateAssetMenu(menuName = "Item/Spells/TestSpell")]
    public class TestSpell : SpellItem
    {
        public override void AttemptToCastSpell(PlayerManager player)
        {
            base.AttemptToCastSpell(player);
            
            if(!CanICastThisSpell(player))
                return;

            if (player._playerNetworkManager._isUsingRightHand.Value)
            {
                player._playerAnimationManager.PlayTargetActionAnimation(_mainHandSpellAnimation,true);   
            }
            else
            {
                player._playerAnimationManager.PlayTargetActionAnimation(_offHandSpellAnimation,true);   

            }
        }
        public override void SuccessfullyCastSpell(PlayerManager player)
        {
            base.SuccessfullyCastSpell(player);
            
            Debug.Log("Cast Spell");
        }
   
        public override void InstantiateWarmUpSpellFX(PlayerManager player)
        {
            base.InstantiateWarmUpSpellFX(player);
            Debug.Log("Cast Spell2");

        }
        public override bool CanICastThisSpell(PlayerManager player)
        {
            if (player._isPerformingAction)
                return false;

            if (player._playerNetworkManager._isJumping.Value)
                return false;

            if (player._playerNetworkManager._currentStamina.Value <= 0)
                return false;
            
            return true;
        }

    }
}