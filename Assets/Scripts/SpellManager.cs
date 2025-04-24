using System;
using SKD.Character;
using UnityEngine;
namespace SKD
{
    public class SpellManager : MonoBehaviour
    {
        [Header("Spell Target")]
        [SerializeField] protected CharacterManager _spellTaregt;

        [Header("VFX")]
        [SerializeField] protected GameObject _impactParticle;
        [SerializeField] protected GameObject _impactParticleFullCharge;

        protected virtual void Awake()
        {

        }
        protected virtual void Start()
        {
        }
        protected virtual void Update()
        {

        }

     
    }
}