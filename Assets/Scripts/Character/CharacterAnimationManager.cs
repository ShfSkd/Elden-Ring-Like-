using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SKD.Character
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        CharacterManager _characterManager;
        float _vertical;
        float _horizontal;
        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            _characterManager._animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            _characterManager._animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimationName,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            _characterManager._applyRootMotion = applyRootMotion;
            _characterManager._animator.CrossFade(targetAnimationName, 0.2f);
            // Can be used to stop character from attempting new actions
            // for example:If you get damaged, and begin performing a damage animation this flag will turn true if you are stunned  
            // we can them check for this before attempting new actions
            _characterManager._isPerfomingAction = isPerformingAction;
            _characterManager._canRotate = canRotate;
            _characterManager._canMove = canMove;

            // Tell the server/host we played an animation, and to play that animation for everybody else present
            _characterManager._characterNetworkManager.NotifyTheServerofActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimationName,applyRootMotion);
        }
    }
}