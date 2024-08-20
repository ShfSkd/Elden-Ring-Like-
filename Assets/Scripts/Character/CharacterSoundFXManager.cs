using SKD.World_Manager;
using System.Collections;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] _damageGrunts;

        [Header("Attack Grunts")]
        [SerializeField] protected AudioClip[] _attackGrunts;

        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>(); 
        }
        public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            _audioSource.PlayOneShot(soundFX, volume);
            // Reset the pitch
            _audioSource.pitch = 1;
            if(randomizePitch )
            {
                _audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }
        public void PlayRollSoundFX()
        {
            _audioSource.PlayOneShot(WorldSoundFXManager.instance._rollSFX);
        }
        public virtual void PlayDamageGrunts()
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(_damageGrunts)); 
        }
        public virtual void PlayAttackGrunt()
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(_attackGrunts)); 
        }
    }
}