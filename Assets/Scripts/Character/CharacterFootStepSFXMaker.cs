using SKD.World_Manager;
using System;
using System.Collections;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterFootStepSFXMaker : MonoBehaviour
    {
        CharacterManager _characterManager;

        AudioSource _audioSource;
        GameObject _steppedOnObject;

        private bool _hasTuchTheGround;
        private bool _hasPlayedFootStepSFX;
        [SerializeField] float _distanceToGround = 0.05f;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _characterManager = GetComponentInParent<CharacterManager>();
        }

        private void FixedUpdate()
        {
            CheckForFootStep();
        }

        private void CheckForFootStep()
        {
            if (_characterManager == null)
                return;

            if (!_characterManager._characterNetworkManager._isMoving.Value)
                return;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, _characterManager.transform.TransformDirection(Vector3.down), out hit, _distanceToGround, WorldUtilityManager.Instance.GetEnviroLayers()))
            {
                _hasTuchTheGround = true;

                if (!_hasTuchTheGround)
                    _steppedOnObject = hit.transform.gameObject;
            }
            else
            {
                _hasTuchTheGround = false;
                _hasPlayedFootStepSFX = false;
                _steppedOnObject = null;
            }
            if (_hasTuchTheGround && !_hasPlayedFootStepSFX)
            {
                _hasTuchTheGround = true;
                PlayFootStepSFX();
            }
        }

        private void PlayFootStepSFX()
        {
            // Here you could play a different SFX depending on the layer of the ground or a tag or such (snow, wood, stone, etc)

            _characterManager._characterSoundFXManager.PlayFootStepSFX(); 
        }
    }
}