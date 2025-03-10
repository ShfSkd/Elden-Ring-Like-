using UnityEngine;
namespace SKD.Character.Player
{
    public class PlayerBodyManager : MonoBehaviour
    {
        [Header("Hair Object")]
        [SerializeField] public GameObject _hair;
       [SerializeField] public GameObject _facialHair;
        
        [Header("Male")]
        [SerializeField] GameObject _maleHead; // Default head when unequipped Armor
        [SerializeField] GameObject[] _maleBody; // Default upper body armor when unequipped Armor (chest, upper right arm, upper left arm)
        [SerializeField] GameObject[] _maleArms; // Default upper when unequipped Armor (lower right arm, right hand,lower left arm, left hand)
        [SerializeField]GameObject [] _maleLegs; // Default upper when unequipped Armor (right leg, left leg)
        [SerializeField] GameObject _maleEyebrows;
        [SerializeField] GameObject _maleFacialHair;
        
        [Header("Female")]
        [SerializeField] GameObject _femaleHead;
        [SerializeField] GameObject[] _femaleBody;
        [SerializeField] GameObject[] _femaleArms;
        [SerializeField]GameObject [] _femaleLegs;
        [SerializeField] GameObject _femaleEyebrows;
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
        }
    }
}