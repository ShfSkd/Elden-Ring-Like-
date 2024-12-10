using SKD.Character.Player;
using UnityEngine;

namespace SKD.Character
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        // What slot is it? (Left hand or right , or hips or back)
        public WeaponModelSlot _weaponSlot;
        public GameObject _currentWeaponModel;

        public void UnloadWeaponModel()
        {
            if (_currentWeaponModel != null)
                Destroy(_currentWeaponModel);
        }

        public void PlaceWeaponModelIntoSlot(GameObject weaponModel)
        {
            _currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }

        public void PlaceWeaponModelInUnequippedSlot(GameObject weaponModel, WeaponClass weaponClass,
            PlayerManager player)
        {
            _currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            switch (weaponClass)
            {
                case WeaponClass.StraightSword:
                    weaponModel.transform.localPosition = new Vector3(0.064f, 0f, -0.06f);
                    weaponModel.transform.localRotation = Quaternion.Euler(194f, 90f, -0.22f);
                    break;
                case WeaponClass.Spear:
                    weaponModel.transform.localPosition = new Vector3(0.064f, 0f, -0.06f);
                    weaponModel.transform.localRotation = Quaternion.Euler(194f, 90f, -0.22f);
                    break;
                case WeaponClass.MediumShield:
                    weaponModel.transform.localPosition = new Vector3(0.074f, -0.002f, 0.069f);
                    weaponModel.transform.localRotation = Quaternion.Euler(-180.235f, 180.202f, -15.65601f);
                    break;
                default:
                    break;
            }
        }
    }
}