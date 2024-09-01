using SKD.Character.AI_Character;
using SKD.World_Manager;
using UnityEngine;

namespace SKD
{
    public class EventTriggerBoosFight : MonoBehaviour
    {
        [SerializeField] int _bossID;
        private void OnTriggerEnter(Collider other)
        {
            AIBossCharacterManager boss = WorldAIManager.Instance.GetBossCharacterByID(_bossID);

            if(boss != null)
            {
                boss.WakeBoss();     
            }
        }
    }
}