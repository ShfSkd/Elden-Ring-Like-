using UnityEngine;

namespace SKD
{
    public class WeaponModelInstantationSlot : MonoBehaviour
    {
        // What slot is it? (Left hand or right , or hips or back)
        public WeaponModelSlot _weaponSlot;
        public GameObject _currentWeaponModel;

        public void UnloadWeaponModel()
        {
            if (_currentWeaponModel != null)
                Destroy(_currentWeaponModel);
        }
        public void LoadWeaponModel(GameObject weaponModel)
        {
            _currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
    }
}
