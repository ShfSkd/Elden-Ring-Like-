using SKD.World_Manager;
using System.Collections;
using UnityEngine;

namespace SKD.Character.AI_Character.Durk
{
    public class AIDurkSoundFXManager : CharacterSoundFXManager
    {
        [Header("Club Whooshes")]
        public AudioClip[] _clubWhooshes;

        [Header("Club Impacts")]
        public AudioClip[] _clubImpacts;

        [Header("Stomp Impacts")]
        public AudioClip[] _stompImpacts;

        public virtual void PlayClubImpactSFX()
        {
            if(_clubImpacts.Length > 0) 
                PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(_clubImpacts));
        }
        public virtual void PlayStompImpact()
        {
            if (_stompImpacts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(_stompImpacts));
        }
    }
} 