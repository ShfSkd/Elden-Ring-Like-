using System;
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
                 case EquipmentModelType.Hat:
                     foreach (var model in player._playerEquipmentManager._hats)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.Hood:
                     foreach (var model in player._playerEquipmentManager._hoods)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.HelmetAcessorie:
                     foreach (var model in player._playerEquipmentManager._helmetAccessories)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.FaceCover:
                     foreach (var model in player._playerEquipmentManager._faceCovers)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.Torso:
                     foreach (var model in player._playerEquipmentManager._maleBodies)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.Back:
                     foreach (var model in player._playerEquipmentManager._backAccessories)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightShoulder:
                     foreach (var model in player._playerEquipmentManager._rightShoulders)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightUpperArm:
                     foreach (var model in player._playerEquipmentManager._rightShoulders)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightElbow:
                     foreach (var model in player._playerEquipmentManager._rightElbows)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightLowerArm:
                     foreach (var model in player._playerEquipmentManager._maleRightLowerArms)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightHand:
                     foreach (var model in player._playerEquipmentManager._maleRightHands)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftShoulder:
                     foreach (var model in player._playerEquipmentManager._leftShoulders)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftUpperArm:
                     foreach (var model in player._playerEquipmentManager._maleLeftUpperArms)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftElbow:
                     foreach (var model in player._playerEquipmentManager._leftElbows)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftLowerArm:
                     foreach (var model in player._playerEquipmentManager._maleLeftLowerArms)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftHand:
                     foreach (var model in player._playerEquipmentManager._maleLeftHands)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.Hips:
                     foreach (var model in player._playerEquipmentManager._maleHips)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.HipsAttachment:
                     foreach (var model in player._playerEquipmentManager._hipAccessories)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightLeg:
                     foreach (var model in player._playerEquipmentManager._maleRightLegs)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightKnee:
                     foreach (var model in player._playerEquipmentManager._rightKnees)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftLeg:
                     foreach (var model in player._playerEquipmentManager._maleLeftLegs)
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftKnee:
                     foreach (var model in player._playerEquipmentManager._leftKnees )
                     {
                         if (model.gameObject.name == _maleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 default:
                    break;
            }
        }
        private void LoadFemaleModel(PlayerManager player)
        {
            // 1. Search through a list of all equipment models based on type (EX: if this is a helmet, we look through all helmets
            // 2. Enable the helmet that match the name 
            
                switch (_equipmentModelType)        
            {
                 case EquipmentModelType.FullHelmet:
                     foreach (var model in player._playerEquipmentManager._femaleHeadFullHelmets)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.Hat:
                     foreach (var model in player._playerEquipmentManager._hats)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.Hood:
                     foreach (var model in player._playerEquipmentManager._hoods)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.HelmetAcessorie:
                     foreach (var model in player._playerEquipmentManager._helmetAccessories)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.FaceCover:
                     foreach (var model in player._playerEquipmentManager._faceCovers)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.Torso:
                     foreach (var model in player._playerEquipmentManager._maleBodies)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.Back:
                     foreach (var model in player._playerEquipmentManager._backAccessories)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightShoulder:
                     foreach (var model in player._playerEquipmentManager._rightShoulders)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightUpperArm:
                     foreach (var model in player._playerEquipmentManager._rightShoulders)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightElbow:
                     foreach (var model in player._playerEquipmentManager._rightElbows)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightLowerArm:
                     foreach (var model in player._playerEquipmentManager._maleRightLowerArms)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightHand:
                     foreach (var model in player._playerEquipmentManager._maleRightHands)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftShoulder:
                     foreach (var model in player._playerEquipmentManager._leftShoulders)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftUpperArm:
                     foreach (var model in player._playerEquipmentManager._maleLeftUpperArms)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftElbow:
                     foreach (var model in player._playerEquipmentManager._leftElbows)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftLowerArm:
                     foreach (var model in player._playerEquipmentManager._maleLeftLowerArms)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftHand:
                     foreach (var model in player._playerEquipmentManager._maleLeftHands)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.Hips:
                     foreach (var model in player._playerEquipmentManager._maleHips)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.HipsAttachment:
                     foreach (var model in player._playerEquipmentManager._hipAccessories)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightLeg:
                     foreach (var model in player._playerEquipmentManager._maleRightLegs)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.RightKnee:
                     foreach (var model in player._playerEquipmentManager._rightKnees)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftLeg:
                     foreach (var model in player._playerEquipmentManager._maleLeftLegs)
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 case EquipmentModelType.LeftKnee:
                     foreach (var model in player._playerEquipmentManager._leftKnees )
                     {
                         if (model.gameObject.name == _femaleEquipmentName)
                         {
                             model.gameObject.SetActive(true);
                         }
                     }
                     break;
                 default:
                    break;
            }
        }
    }
}