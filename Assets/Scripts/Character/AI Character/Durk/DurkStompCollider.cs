using SKD.Colliders;
using SKD.Effects;
using SKD.World_Manager;
using SKD.WorldManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.Character.AI_Character.Durk
{
    public class DurkStompCollider : DamageCollider
    {
        [SerializeField] AIDurkCharacterManager _durkCharacterManager;

        protected override void Awake()
        {
            base.Awake();

            _durkCharacterManager = GetComponentInParent<AIDurkCharacterManager>();
        }
        public void StompAttack()
        {
            GameObject stompVFX = Instantiate(_durkCharacterManager._aIDurkCombatManager._durkImpactVFX, transform);    
            Collider[] colliders = Physics.OverlapSphere(transform.position, _durkCharacterManager._aIDurkCombatManager._stompAttackAOERadius, WorldUtilityManager.Instance.GetCharacterLayers());
            List<CharacterManager> charactersDamagedList = new List<CharacterManager>();

            foreach (Collider collider in colliders)
            {
                CharacterManager character = collider.GetComponent<CharacterManager>();

                if (character != null)
                {
                    if (charactersDamagedList.Contains(character))
                        continue;

                    charactersDamagedList.Add(character);

                    // we don't want the durk to it himself
                    if (character == _durkCharacterManager)
                        continue;

                    // We only process damage if the character "IsOwner" so that they only get damaged if the collider connects on their client
                    // Meaning if you are hit on the hosts screen but not on your own, you will not be hit 
                    if (character.IsOwner)
                    {
                        // Check for block 
                        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance._takeDamageEffect);
                        damageEffect._physicalDamage = _durkCharacterManager._aIDurkCombatManager.GetStompDamage();
                        damageEffect._poiseDamage = _durkCharacterManager._aIDurkCombatManager.GetStompDamage();

                        character._characterEffectsManager.ProceesInstanceEffect(damageEffect);
                    }
                }


            }
        }
    }
}