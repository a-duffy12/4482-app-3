using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    AudioSource inventorySource;

    // Start is called before the first frame update
    void Start()
    {
        inventorySource = GetComponentInChildren<AudioSource>();
        inventorySource.playOnAwake = false;
        inventorySource.spatialBlend = 1f;
        inventorySource.volume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region input functions

    public void Shoot(InputAction.CallbackContext con)
    {
       
    }

    public void Aim(InputAction.CallbackContext con)
    {
       
    }

    public void Reload(InputAction.CallbackContext con)
    {
       
    }

    public void Item1(InputAction.CallbackContext con)
    {
       
    }

    public void Item2(InputAction.CallbackContext con)
    {
       
    }

    public void Item3(InputAction.CallbackContext con)
    {
       
    }

    public void Item4(InputAction.CallbackContext con)
    {
       
    }

    #endregion input functions
}
