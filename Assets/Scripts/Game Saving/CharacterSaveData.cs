using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.Game_Saving
{
    [System.Serializable]
    // Since we want to reference this data for every save file, this script in not MonoBehavoiur and instead Serializable
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int _sceneIndex = 1;

        [Header("Character Name")]
        public string _characterName = "Character";

        [Header("Time Played")]
        public float _secondsPlayed;

        // Question: Why not to use vector3?
        // Answer: we can only use save data from "basic" variables types(float,Int,string etc..)
        [Header("World Coordinates")]
        public float _xPosition;
        public float _yPosition;
        public float _zPosition;

        [Header("Resources")]
        public int _currentHealth;
        public float _currentStamina;

        [Header("Stats")]
        public int _vitality;
        public int _endurance;
    }
}
