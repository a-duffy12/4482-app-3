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
    public bool nsfwEnabled;
    public string crosshairColor;
    public string keybinds;
    
    [Header("Game Status")]
    public int levelCount;
    public bool assaultRifleUnlocked;
    public bool shotgunUnlocked;
    public bool sniperUnlocked;
    public bool flamethrowerUnlocked;
    public bool knifeUnlocked;
    public bool grenadeUnlocked;
    public bool dashUnlocked;
    public bool rewindUnlocked;

    // function to get all data that needs saving
    public GameData()
    {
        fieldOfView = Config.fieldOfView;
        sensitivity = Config.sensitivity;
        difficultyLevel = (int) Config.difficultyLevel;
        difficultyMod = Config.difficultyMod;
        nsfwEnabled = Config.nsfwEnabled;
        crosshairColor = Config.crosshairColor;
        keybinds = Config.keybinds;
        
        levelCount = Config.levelCount;
        assaultRifleUnlocked = Config.assaultRifleUnlocked;
        shotgunUnlocked = Config.shotgunUnlocked;
        sniperUnlocked = Config.sniperUnlocked;
        flamethrowerUnlocked = Config.flamethrowerUnlocked;
        knifeUnlocked = Config.knifeUnlocked;
        grenadeUnlocked = Config.grenadeUnlocked;
        dashUnlocked = Config.dashUnlocked;
        rewindUnlocked = Config.rewindUnlocked;
    }
}