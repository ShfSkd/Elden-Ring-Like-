using SKD.Character;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace SKD.World_Manager
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager Instance;

        [Header("Boss Track")]
        [SerializeField] AudioSource _bossIntroPlayer;
        [SerializeField] AudioSource _bossLoopPlayer;

        [Header("Damage Sounds")]
        public AudioClip[] _physicalDamageSFX;

        [Header("Action Sounds")]
        public AudioClip _rollSFX;
        public AudioClip _pickUpItemSFX;
        public AudioClip _stanceBreakSFX;
        public AudioClip _criticalStrikeSFX;
        public AudioClip[] _releaseArrowSFX;
        public AudioClip[] _notchArrowSFX;
        public AudioClip _heallingFlaskFX;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void PlayBossTrack(AudioClip introTrack, AudioClip loopTrack)
        {
            _bossIntroPlayer.volume = 1;
            _bossIntroPlayer.clip = introTrack;
            _bossIntroPlayer.loop = false;
            _bossIntroPlayer.Play();

            _bossLoopPlayer.volume = 1;
            _bossLoopPlayer.clip = loopTrack;
            _bossLoopPlayer.loop = true;
            _bossLoopPlayer.PlayDelayed(_bossIntroPlayer.clip.length);
        }
        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);

            return array[index];
        }

        public void StopBossMusic()
        {
            StartCoroutine(FadeOutBossMusicThenStop());
        }
        private IEnumerator FadeOutBossMusicThenStop()
        {
            while (_bossLoopPlayer.volume > 0)
            {
                _bossLoopPlayer.volume -= Time.deltaTime;
                _bossIntroPlayer.volume -= Time.deltaTime;
                yield return null;
            }
            _bossIntroPlayer.Stop();
            _bossLoopPlayer.Stop();
        }
    }
}