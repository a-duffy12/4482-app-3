using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Settings")]
    public float fieldOfView;
    public float sensitivity;
    public int difficultyLevel;
    public float difficultyMod;
    
    public float levelCount;

    // function to get all data that needs saving
    public GameData()
    {
        fieldOfView = Config.fieldOfView;
        sensitivity = Config.sensitivity;
        difficultyLevel = (int) Config.difficultyLevel;
        difficultyMod = Config.difficultyMod;
        
        levelCount = Config.levelCount;
    }
}