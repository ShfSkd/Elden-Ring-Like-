using System.Collections;
using UnityEngine;

namespace SKD.World_Manager
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Damage Sounds")]
        public AudioClip[] _physicalDamageSFX;

        [Header("Action Sounds")]
        public AudioClip _rollSFX;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);

            return array[index];
        }
    }
}