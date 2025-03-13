using System;
using UnityEngine;
namespace SKD.Character.Player
{
    public class PlayerBodyManager : MonoBehaviour
    {
        PlayerManager _player;
         
        [Header("Hair Object")]
        [SerializeField] public GameObject _hair;
        [SerializeField] public GameObject _facialHair;

        [Header("Male")]
        [SerializeField] GameObject _maleObject; // The master male gameObject parent
        [SerializeField] GameObject _maleHead;// Default head when unequipped Armor
        [SerializeField] GameObject[] _maleBody;// Default upper body armor when unequipped Armor (chest, upper right arm, upper left arm)
        [SerializeField] GameObject[] _maleArms;// Default upper when unequipped Armor (lower right arm, right hand,lower left arm, left hand)
        [SerializeField] GameObject[] _maleLegs;// Default upper when unequipped Armor (hips, right leg, left leg)
        [SerializeField] GameObject _maleEyebrows;
        [SerializeField] GameObject _maleFacialHair;

        [Header("Female")]
        [SerializeField] GameObject _femaleObject; // The master female gameObject parent
        [SerializeField] GameObject _femaleHead;
        [SerializeField] GameObject[] _femaleBody;
        [SerializeField] GameObject[] _femaleArms;
        [SerializeField] GameObject[] _femaleLegs;
        [SerializeField] GameObject _femaleEyebrows;

        void Awake()
        {
            _player = GetComponent<PlayerManager>();
        }
        public void EnableHead()
        {
            _maleHead.SetActive(true);
            _femaleHead.SetActive(true);
            _maleEyebrows.SetActive(true);
            _femaleEyebrows.SetActive(true);
        }
        public void DisableHead()
        {
            _maleHead.SetActive(false);
            _femaleHead.SetActive(false);
            _maleEyebrows.SetActive(false);
            _femaleEyebrows.SetActive(false);
        }
        public void EnableHair()
        {
            _hair.SetActive(true);
        }
        public void DisableHair()
        {
            _hair.SetActive(false);
        }
        public void EnableFacialHair()
        {
            _facialHair.SetActive(true);
        }
        public void DisableFacialHair()
        {
            _facialHair.SetActive(false);
        }
        public void EnableBody()
        {
            foreach (var model in _maleBody)
            {
                model.SetActive(true);
            }
            foreach (var model in _femaleBody)
            {
                model.SetActive(true);
            }
        }
        public void EnableLowerBody()
        {
            foreach (var model in _maleLegs)
            {
                model.SetActive(true);
            }
            foreach (var model in _femaleLegs)
            {
                model.SetActive(true);
            }
        }
        public void EnableArms()
        {
            foreach (var model in _maleArms)
            {
                model.SetActive(true);
            }
            foreach (var model in _femaleArms)
            {
                model.SetActive(true);
            }
        }
        public void DisableBody()
        {
            foreach (var model in _maleBody)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleBody)
            {
                model.SetActive(false);
            }
        }
        public void DisableLowerBody()
        {
            foreach (var model in _maleLegs)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleLegs)
            {
                model.SetActive(false);
            }
        }
        public void DisableArms()
        {
            foreach (var model in _maleArms)
            {
                model.SetActive(false);
            }
            foreach (var model in _femaleArms)
            {
                model.SetActive(false);
            }
        }
        public void ToggleBodyType(bool isMale)
        {
            if (isMale)
            {
                _maleObject.SetActive(true);
                _femaleObject.SetActive(false);
            }
            else 
            {
                _maleObject.SetActive(false);
                _femaleObject.SetActive(true);
            }
            _player._playerEquipmentManager.EquipArmor();
        }
    }
}