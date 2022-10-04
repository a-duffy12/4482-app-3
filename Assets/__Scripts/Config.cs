using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Config
{
    [Header("Settings")]
    [Range(60, 110)] public static float fieldOfView = 90f;
    [Range(0.1f, 100)] public static float sensitivity = 20f;
    [Range(0, 3)] public static Difficulty.Level difficultyLevel = Difficulty.Level.Normal;
    [Range(0, 2)] public static float difficultyMod = 0.4f;

    [Header("Jumping")]
    [Range(0f, 100)] public static float gravity = -9.81f;
    [Range(0f, 1f)] public static float jumpWindow = 0.1f;
    [Range(0f, 20)] public static float jumpSpeed = 6.5f;
    [Range(0f, 20)] public static float airAcceleration = 16f;
    [Range(0f, 20)] public static float airFriction = 0.4f;
    
    [Header("Ground Movement")]
    [Range(0f, 20)] public static float sprintSpeed = 12f;
    [Range(0f, 20)] public static float acceleration = 14f;
    [Range(0f, 20)] public static float deceleration = 10f;
    [Range(0f, 20)] public static float crouchSpeed = 6f;
    [Range(0f, 20)] public static float crouchAcceleration = 11f;
    [Range(0f, 20)] public static float friction = 6f;

    [Header("Speciality Movement")]
    [Range(0f, 1000)] public static float dashSpeed = 100f;
    [Range(0f, 10)] public static float dashCooldown = 2f;
    [Range(0f, 10)] public static float rewindAmount = 3f;
    [Range(0f, 60)] public static float rewindCooldown = 20f;

    [Header("Modifiers")]
    [Range(0f, 2f)] public static float enemyAggroRadiusModifier = 1f;

    [Header("Game Status")]
    [HideInInspector] public static int levelCount = 0;
    [HideInInspector] public static bool assaultRifleUnlocked = true;
    [HideInInspector] public static bool shotgunUnlocked = true;
    [HideInInspector] public static bool sniperUnlocked = true;
    [HideInInspector] public static bool flamethrowerUnlocked = true;
    [HideInInspector] public static bool knifeUnlocked = true;
    [HideInInspector] public static bool grenadeUnlocked = true;
    [HideInInspector] public static bool rewindUnlocked = true;

    [Header("Assault Rifle")]
    [HideInInspector] public static float assaultRifleFireRate = 10.0f;
    [HideInInspector] public static float assaultRifleReloadTime = 3.0f;
    [HideInInspector] public static int assaultRifleMaxAmmo = 400;
    [HideInInspector] public static float assaultRifleDamage = 25;
    [HideInInspector] public static float assaultRifleFireVelocity = 4500;
    [HideInInspector] public static float assaultRifleRange = 150;

    [Header("Shotgun")]
    [HideInInspector] public static float shotgunFireRate = 1.2f;
    [HideInInspector] public static float shotgunReloadTime = 2.6f;
    [HideInInspector] public static int shotgunMaxAmmo = 60;
    [HideInInspector] public static float shotgunDamage = 14;
    [HideInInspector] public static float shotgunFireVelocity = 1200;
    [HideInInspector] public static float shotgunRange = 20;

    [Header("Sniper")]
    [HideInInspector] public static float sniperFireRate = 0.8f;
    [HideInInspector] public static float sniperReloadTime = 4.0f;
    [HideInInspector] public static int sniperMaxAmmo = 45;
    [HideInInspector] public static float sniperDamage = 145;
    [HideInInspector] public static float sniperFireVelocity = 7000;
    [HideInInspector] public static float sniperRange = 500;
    [HideInInspector] public static bool sniperScopedIn = false;
    [HideInInspector] public static float sniperAdsFov = 30;
    [HideInInspector] public static float sniperAdsSensitivityMod = 0.6f;

    [Header("Flamethrower")]
    [HideInInspector] public static float flamethrowerFireRate = 20f;
    [HideInInspector] public static float flamethrowerReloadTime = 4.5f;
    [HideInInspector] public static int flamethrowerMaxAmmo = 500;
    [HideInInspector] public static float flamethrowerDamage = 10;
    [HideInInspector] public static float flamethrowerFireVelocity = 1000;
    [HideInInspector] public static float flamethrowerRange = 25;

    [Header("Cyclops")]
    [HideInInspector] public static float ogreMaxHp = 100;
    [HideInInspector] public static float ogreMovementSpeed = 9f;
    [HideInInspector] public static float ogreAggroDistance = 30f;
    [HideInInspector] public static float ogreAttackDistance = 1.0f;
    [HideInInspector] public static float ogreDamage = 60f;

    public static void GetSaveData()
    {
        GameData savedData = SaveLoad.LoadData(); // load serialized data

        if (savedData != null)
        {
            Config.fieldOfView = savedData.fieldOfView;
            Config.sensitivity = savedData.sensitivity;
            Config.difficultyLevel = savedData.difficultyLevel == 0 ? Difficulty.Level.Easy : savedData.difficultyLevel == 1 ? Difficulty.Level.Normal : savedData.difficultyLevel == 2 ? Difficulty.Level.Hard : savedData.difficultyLevel == 3 ? Difficulty.Level.Fun : Difficulty.Level.Normal;
            Config.difficultyMod = savedData.difficultyMod;
        
            Config.levelCount = savedData.levelCount;
            Config.assaultRifleUnlocked = savedData.assaultRifleUnlocked;
            Config.shotgunUnlocked = savedData.shotgunUnlocked;
            Config.sniperUnlocked = savedData.sniperUnlocked;
            Config.flamethrowerUnlocked = savedData.flamethrowerUnlocked;
            Config.knifeUnlocked = savedData.knifeUnlocked;
            Config.grenadeUnlocked = savedData.grenadeUnlocked;
            Config.rewindUnlocked = savedData.rewindUnlocked;
        }
    }
}