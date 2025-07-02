using SKD.Character.Player;
using SKD.Items.Quick_Item_Slot;
using SKD.UI.PlayerUI;
using SKD.World_Manager;
using SKD.WorldManager;
using UnityEngine;
using UnityEngine.Serialization;
namespace Items.Flasks
{
    [CreateAssetMenu(menuName = "Items/Consumable/Flask")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("Flasks Type")]
        public bool _isHealthFlask;

        [Header("Restoration Value")]
        [SerializeField] int _flaskRestoration = 50;

        [Header("Empty Flask")]
        public GameObject _emptyFlaskItem;
        public string _emptyFlaskAnimation;


        public override bool CanIUseThisItem(PlayerManager player)
        {
            if (!player._playerCombatManager._isUsingItem && player._isPerformingAction)
                return false;

            if (player._playerNetworkManager._isAttacking.Value)
                return false;

            return true;
        }


        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player))
                return;

            // Health flaks check
            if (_isHealthFlask && player._playerNetworkManager._remainingHealthFlasks.Value <= 0)
            {
                if (player._playerCombatManager._isUsingItem)
                    return;

                player._playerCombatManager._isUsingItem = true;

                if (player.IsOwner)
                {
                    player._playerAnimationManager.PlayTargetActionAnimation(_emptyFlaskAnimation, false, false, true, true, false);
                    player._playerNetworkManager.HideWeaponsServerRpc();
                }

                Destroy(player._playerEffectsManager._activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(_emptyFlaskItem, player._playerEquipmentManager._rightHandWeaponSlot.transform);
                player._playerEffectsManager._activeQuickSlotItemFX = emptyFlask;

                return;
            }

            // Focus points Flaks check
            if (!_isHealthFlask && player._playerNetworkManager._remainingFocusPointsFlasks.Value <= 0)
            {
                if (player._playerCombatManager._isUsingItem)
                    return;

                player._playerCombatManager._isUsingItem = true;

                if (player.IsOwner)
                {
                    player._playerAnimationManager.PlayTargetActionAnimation(_emptyFlaskAnimation, false, false, true, true, false);
                    player._playerNetworkManager.HideWeaponsServerRpc();
                }

                Destroy(player._playerEffectsManager._activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(_emptyFlaskItem, player._playerEquipmentManager._rightHandWeaponSlot.transform);
                player._playerEffectsManager._activeQuickSlotItemFX = emptyFlask;
                return;
            }

            // Check for chugging
            if (player._playerCombatManager._isUsingItem)
            {
                if (player.IsOwner)
                    player._playerNetworkManager._isChugging.Value = true;

                return;
            }

            player._playerCombatManager._isUsingItem = true;
            player._playerEffectsManager._activeQuickSlotItemFX = Instantiate(_itemModel, player._playerEquipmentManager._rightHandWeaponSlot.transform);

            if (player.IsOwner)
            {
                player._playerAnimationManager.PlayTargetActionAnimation(_useItemAnimation, false, false, true, true, false);
                player._playerNetworkManager.HideWeaponsServerRpc();
            }
        }
        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (player.IsOwner)
            {
                if (_isHealthFlask)
                {
                    player._playerNetworkManager._currentHealth.Value += _flaskRestoration;
                    player._playerNetworkManager._remainingHealthFlasks.Value -= 1;
                }
                else
                {
                    player._playerNetworkManager._currentFocusPoints.Value += _flaskRestoration;
                    player._playerNetworkManager._remainingFocusPointsFlasks.Value -= 1;
                }

            }
            if (_isHealthFlask && player._playerNetworkManager._remainingHealthFlasks.Value <= 0)
            {
                Destroy(player._playerEffectsManager._activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(_emptyFlaskItem, player._playerEquipmentManager._rightHandWeaponSlot.transform);
                player._playerEffectsManager._activeQuickSlotItemFX = emptyFlask;
            }
            else if (_isHealthFlask && player._playerNetworkManager._remainingFocusPointsFlasks.Value <= 0)
            {
                Destroy(player._playerEffectsManager._activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(_emptyFlaskItem, player._playerEquipmentManager._rightHandWeaponSlot.transform);
                player._playerEffectsManager._activeQuickSlotItemFX = emptyFlask;
            }
            PlayHealingFX(player);
        }
        private void PlayHealingFX(PlayerManager player)
        {
            Instantiate(WorldCharacterEffectsManager.Instance._healingFlaskVFX, player.transform);
            player._characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance._heallingFlaskFX);
        }

        public override int GetCurrentAmount(PlayerManager player)
        {
            int currentAmount = 0;

            if (_isHealthFlask)
                currentAmount = player._playerNetworkManager._remainingHealthFlasks.Value;
            else
                currentAmount = player._playerNetworkManager._remainingFocusPointsFlasks.Value;
            
            return currentAmount;
        }
    }
}