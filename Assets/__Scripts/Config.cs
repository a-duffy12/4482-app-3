using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Config
{
    [Header("Settings")]
    [Range(0.01f, 10)] public static float sensitivity = 1f;
    [Range(60, 110)] public static float fieldOfView = 60f;

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
}