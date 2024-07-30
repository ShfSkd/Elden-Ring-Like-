using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SKD.Game_Saving
{
    public class SaveFileDataWriter 
    {
        public string _saveDataDirectoryPath = "";
        public string _saveFileName = "";

        // Before we create a new save file, we must check to see if one of this character slot already exists(max10 characters slots)
        public bool ChechTooSeeFileExists()
        {
            if (File.Exists(Path.Combine(_saveDataDirectoryPath, _saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Used to delete character save files
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(_saveDataDirectoryPath, _saveFileName));
        }
        // Used to create a save file upon starting a new game
        public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData)
        {
            // Make a path to the save file (a location on the machine)
            string savePath = Path.Combine(_saveDataDirectoryPath, _saveFileName);

            try
            {
                // Create the directory the file will be written to, if the file does not already exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("Creating Saving File, At Save Path: " + savePath);

                // Serielize the C# game data object unto JSON
                string dataToStore = JsonUtility.ToJson(characterSaveData, true);

                // Write the file to our system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error Whilst Trying To Save Character Data, Game not Saved! " + savePath + "\n" + ex);
            }
        }
        // Used to create a load file upon loading a previous game
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterSaveData = null;
            // Make a path to the load file (a location on the machine)
            string loadPath = Path.Combine(_saveDataDirectoryPath, _saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    // Deserialize the data from JSON back to Unity
                    characterSaveData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception)
                {
                    Debug.LogError("");
                }
            }
            return characterSaveData;
        }
    }
}