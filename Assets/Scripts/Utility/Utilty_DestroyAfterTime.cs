using System.Collections;
using UnityEngine;

namespace SKD.Utility
{
    public class Utilty_DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] float _timeUntilDestroyd = 5f;

        private void Start()
        {
            Destroy(gameObject,_timeUntilDestroyd);
        }

    }
}