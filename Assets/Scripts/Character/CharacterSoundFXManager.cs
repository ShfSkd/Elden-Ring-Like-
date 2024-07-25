using SKD.World_Manager;
using System.Collections;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>(); 
        }
        public void PlayRollSoundFX()
        {
            _audioSource.PlayOneShot(WorldSoundFXManager.instance._rollSFX);
        }
    }
}