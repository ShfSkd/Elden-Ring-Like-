using System.Collections;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;

        public Camera _cameraObject;

        private void Awake()
        {
            if(instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}