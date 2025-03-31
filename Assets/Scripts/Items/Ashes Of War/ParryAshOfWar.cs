using System;
using SKD.Character.Player;
using UnityEngine;
namespace SKD.Items.AshesOfWar
{
    [CreateAssetMenu(menuName = "Items/Ash Of War/Parry")]
    public class ParryAshOfWar : AshOfWar
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction);

            if (!CanIUseThisAbility(playerPerformingAction))
                return;

            DeductStaminaCost(playerPerformingAction);
            DeductFoucusPointCost(playerPerformingAction);
            ChooseParryTypeBasedOnWeapon(playerPerformingAction);
        }
        public override bool CanIUseThisAbility(PlayerManager playerPerformingAction)
        {
            if (playerPerformingAction._isPerformingAction)
                return false;
            
            if (playerPerformingAction._playerNetworkManager._isJumping.Value)
                return false;
            
            if (!playerPerformingAction._playerLocomotionManager._isGrounded)
                return false;
            
            if (playerPerformingAction._playerNetworkManager._currentStamina.Value <= 0)
                return false;

            return true;


        }

        // Smaller weapon performs faster parries 
        private void ChooseParryTypeBasedOnWeapon(PlayerManager playerPerformingAction)
        {
            WeaponItem weaponBeingUsed = playerPerformingAction._playerCombatManager._currentWeaponBeingUsed;

            switch (weaponBeingUsed._weaponClass)
            {

                case WeaponClass.StraightSword:
                    break;
                case WeaponClass.Spear:
                    break;
                case WeaponClass.MediumShield:
                    playerPerformingAction._playerAnimationManager.PlayTargetActionAnimation("Slow_Parry_01",true);
                    break;
                case WeaponClass.Fist:
                    break;
                case WeaponClass.LightShield:
                    playerPerformingAction._playerAnimationManager.PlayTargetActionAnimation("Fast_Parry_01",true);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}