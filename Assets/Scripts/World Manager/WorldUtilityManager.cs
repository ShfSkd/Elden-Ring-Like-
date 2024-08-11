using System.Collections;
using UnityEngine;

namespace SKD.World_Manager
{
    public class WorldUtilityManager : MonoBehaviour
    {
        private static WorldUtilityManager instance;
        public static WorldUtilityManager Instance { get { return instance; } }

        [Header("Layers")]
        [SerializeField] LayerMask _characterLayers;
        [SerializeField] LayerMask _enviroLayers;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

            }
            else
            {
                Destroy(gameObject);
            }

        }

        public LayerMask GetCharacterLayers()
        {
            return _characterLayers;
        }
        public LayerMask GetEnviroLayers()
        {
            return _enviroLayers;
        }

    }
}