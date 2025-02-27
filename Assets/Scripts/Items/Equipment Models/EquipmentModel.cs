using SKD.Character.Player;
using UnityEngine;
namespace SKD.Items.Equipment_Models
{
    [CreateAssetMenu(menuName = "Equipment Model")]
    public class EquipmentModel : ScriptableObject
    {
        public EquipmentModelType _equipmentModelType;
        public string _maleEquipmentName;
        public string _femaleEquipmentName;

        public void LoadModel(PlayerManager player, bool isMale)
        {
            if (isMale)
            {
                LoadMaleModel(player);
            }
            else
            {
                LoadFemaleModel(player);
            }
        }

        private void LoadMaleModel(PlayerManager player)
        {
            // 1. Search through a list of all equipment models based on type (EX: if this is a helmet, we look through all helmets
            // 2. Enable the helmet that match the name 

            switch (_equipmentModelType)        
            {
                 case EquipmentModelType.FullHelmet:
                     foreach (var model in player._playerEquipmentManager._maleHeadFullHelmets)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
            }
        }
        private void LoadFemaleModel(PlayerManager player)
        {
            // 1. Search through a list of all equipment models based on type (EX: if this is a helmet, we look through all helmets
            // 2. Enable the helmet that match the name 
        }
    }
}