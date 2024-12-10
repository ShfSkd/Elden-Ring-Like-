using SKD.Character.AI_Character;
using SKD.Character;
using SKD.Effects;
using SKD.WorldManager;
using UnityEngine;

namespace SKD.Colliders
{
    public class DurkClubDamageCollider : DamageCollider
    {
        [SerializeField] AIBossCharacterManager _durkCharacter;

        protected override void Awake()
        {
            base.Awake();

            _damageCollider = GetComponent<Collider>();
            _durkCharacter = GetComponentInParent<AIBossCharacterManager>();
        }
        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // We don't want to damage the same target more then once in a single attack. So we add them to a list that check before applying damage 
            if (_charactersDamagedList.Contains(damageTarget))
                return;

            _charactersDamagedList.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance._takeDamageEffect);
            damageEffect._physicalDamage = _physicalDamage;
            damageEffect._magicDamage = _magicDamage;
            damageEffect._fireDamage = _fireDamage;
            damageEffect._holyDamage = _holyDamage;
            damageEffect._poiseDamage = _poiseDamage;
            damageEffect._lightingDamage = _lightningDamage;
            damageEffect._constantPoint = _contactPoint;
            damageEffect._angleHitFrom = Vector3.SignedAngle(_durkCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);

            // Option 1: 
            // This will apply damage if the AI hits its target on the host side regardless of how it looks an any other client side 
            /*  if (_undeadCharacter.IsOwner)
              {
                  // Send a damage request from the server
                  damageTarget._characterNetworkManager.NotifyTheServerofCharacterDamageServerRpc(damageTarget.NetworkObjectId, _undeadCharacter.NetworkObjectId,
                      damageEffect._physicalDamage,
                      damageEffect._magicDamage,
                      damageEffect._fireDamage,
                      damageEffect._holyDamage,
                      damageEffect._lightnigamage,
                      damageEffect._poiseDamage,
                      damageEffect._angleHitFrom,
                      damageEffect._constantPoint.x,
                      damageEffect._constantPoint.y,
                      damageEffect._constantPoint.z);
              }*/
            // Option 2: 
            // This will apply damage if the AI hits its target on the connected characters side regardless of how it looks on any other client side
            if (damageTarget.IsOwner)
            {
                damageTarget._characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    _durkCharacter.NetworkObjectId,
                    damageEffect._physicalDamage,
                     damageEffect._magicDamage,
                    damageEffect._fireDamage,
                    damageEffect._holyDamage,
                    damageEffect._lightingDamage,
                    damageEffect._poiseDamage,
                    damageEffect._angleHitFrom,
                    damageEffect._constantPoint.x,
                    damageEffect._constantPoint.y,
                    damageEffect._constantPoint.z);
            }

            //damageTarget._characterEffectsManager.ProceesInstanceEffect(damageEffect);
        }
    }
}