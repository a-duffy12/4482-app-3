using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Config
{
    [Header("Settings")]
    [Range(60, 110)] public static float fieldOfView = 60f;
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

    [HideInInspector] public static float levelCount = 0f;

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
        }
    }
}