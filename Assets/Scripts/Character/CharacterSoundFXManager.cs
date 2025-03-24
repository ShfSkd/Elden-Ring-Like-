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

        [Header("FootSteps")]
        [SerializeField] protected AudioClip[] _footStep;

        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            _audioSource.PlayOneShot(soundFX, volume);
            // Reset the pitch
            _audioSource.pitch = 1;
            if (randomizePitch)
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
            if (_damageGrunts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(_damageGrunts));
        }
        public virtual void PlayAttackGrunt()
        {
            if (_damageGrunts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(_attackGrunts));
        }

        public virtual void PlayFootStepSFX()
        {
            if (_footStep.Length > 0)
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray
                    (_footStep));
        }
        public virtual void PlayStanceBreakSoundFX()
        {
            _audioSource.PlayOneShot(WorldSoundFXManager.instance._stanceBreakSFX);
        }
        public virtual void PlayBlockingSFX()
        {

        }
    }
}