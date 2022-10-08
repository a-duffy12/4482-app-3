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
    public static bool nsfwEnabled = true;

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
    [Range(0f, 10)] public static float dashCooldown = 1.5f;
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

    [Header("Player")]
    [HideInInspector] public static float playerMaxHp = 100f;
    [HideInInspector] public static float assaultRifleHpReturnMod = -0.05f;
    [HideInInspector] public static float shotgunHpReturnMod = -0.04f;
    [HideInInspector] public static float sniperRifleHpReturnMod = -0.005f;
    [HideInInspector] public static float flamethrowerRifleHpReturnMod = -0.1f;
    [HideInInspector] public static float assaultRifleMaxHpReturn = 20f;
    [HideInInspector] public static float shotgunHpMaxReturn = 20f;
    [HideInInspector] public static float sniperRifleMaxHpReturn = 10f;
    [HideInInspector] public static float flamethrowerRifleMaxHpReturn = 50f;

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
    [HideInInspector] public static float sniperUnAdsDamageMod = 0.5f;
    [HideInInspector] public static float sniperAdsFov = 30;
    [HideInInspector] public static float sniperAdsSensitivityMod = 0.6f;

    [Header("Flamethrower")]
    [HideInInspector] public static float flamethrowerFireRate = 20f;
    [HideInInspector] public static float flamethrowerReloadTime = 4.5f;
    [HideInInspector] public static int flamethrowerMaxAmmo = 500;
    [HideInInspector] public static float flamethrowerDamage = 8.0f;
    [HideInInspector] public static float flamethrowerFireVelocity = 1000;
    [HideInInspector] public static float flamethrowerRange = 25;
    [HideInInspector] public static float flamethrowerBurnTime = 5.0f;
    [HideInInspector] public static float flamethrowerBurnDamage = 0.5f;

    [Header("Grenade")]
    [HideInInspector] public static float grenadeCooldown = 10.0f;
    [HideInInspector] public static float grenadeThrowForce = 1800f;
    [HideInInspector] public static float grenadeFuseTime = 1.8f;
    [HideInInspector] public static float grenadeBlastRadius = 3.0f;
    [HideInInspector] public static float grenadeDamage = 10f;
    [HideInInspector] public static float grenadeKnockbackForce = 500f;
    [HideInInspector] public static float grenadeStunDuration = 2.0f;

    [Header("Ogre")]
    [HideInInspector] public static string ogreName = "Ogre";
    [HideInInspector] public static float ogreMaxHp = 200;
    [HideInInspector] public static float ogreDeathPosition = 0.5f;
    [HideInInspector] public static float ogreMovementSpeed = 9f;
    [HideInInspector] public static float ogreAggroDistance = 50f;
    [HideInInspector] public static float ogreAttackDistance = 2.4f;
    [HideInInspector] public static float ogreDamage = 60f;
    [HideInInspector] public static float ogreAttackRate = 0.6f;

    [Header("Demon")]
    [HideInInspector] public static string demonName = "Demon";
    [HideInInspector] public static float demonMaxHp = 150;
    [HideInInspector] public static float demonDeathPosition = 0.5f;
    [HideInInspector] public static float demonMovementSpeed = 7f;
    [HideInInspector] public static float demonAggroDistance = 60f;
    [HideInInspector] public static float demonMinDistance = 20f;
    [HideInInspector] public static float demonMaxDistance = 30f;
    [HideInInspector] public static float demonDamage = 50f;
    [HideInInspector] public static float demonAttackRate = 0.3f;
    [HideInInspector] public static float demonStartleDelay = 2.0f;
    [HideInInspector] public static float demonFireballSpeed = 1800f;
    [HideInInspector] public static float demonFireballFlyTime = 5.0f;

    public static void GetSaveData()
    {
        GameData savedData = SaveLoad.LoadData(); // load serialized data

        if (savedData != null)
        {
            Config.fieldOfView = savedData.fieldOfView;
            Config.sensitivity = savedData.sensitivity;
            Config.difficultyLevel = savedData.difficultyLevel == 0 ? Difficulty.Level.Easy : savedData.difficultyLevel == 1 ? Difficulty.Level.Normal : savedData.difficultyLevel == 2 ? Difficulty.Level.Hard : savedData.difficultyLevel == 3 ? Difficulty.Level.Fun : Difficulty.Level.Normal;
            Config.difficultyMod = savedData.difficultyMod;
            Config.nsfwEnabled = savedData.nsfwEnabled;
        
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