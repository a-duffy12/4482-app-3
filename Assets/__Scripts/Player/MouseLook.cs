using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public Camera firstPersonCamera;
    [SerializeField] private float sensMod; // tweak this to get sens close to source values

    float xRotation = 0f;

    private float xMouse;
    private float yMouse;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Look(xMouse, yMouse);
    }

    void Look(float x, float y)
    {
        float xLook = x * Config.sensitivity * sensMod * Time.deltaTime;
        float yLook = y * Config.sensitivity * sensMod * Time.deltaTime;
        
        xRotation -= yLook;
        xRotation = Mathf.Clamp(xRotation, -90f, 75f); // restrict up and down head movement

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // apply y movement as rotation to the head
        playerBody.Rotate(Vector3.up * xLook); // apply x movement as rotation to the body
    }

    #region input

    public void MouseX(InputAction.CallbackContext con)
    {
        xMouse = con.ReadValue<float>();
    }

    public void MouseY(InputAction.CallbackContext con)
    {
        yMouse = con.ReadValue<float>();
    }

    #endregion input
}
