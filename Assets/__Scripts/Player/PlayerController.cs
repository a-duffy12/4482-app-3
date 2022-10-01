using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region public properties

    public CharacterController controller;
    public Transform player;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [Range(0f, 0.5f)] [SerializeField] private float groundRadius = 0.15f;

    [Header("Audio")]
    // move audio
    // crouch audio
    // jump audio
    // dash audio
    // rewind audio

    #endregion public properties

    #region private properties

    private bool isGrounded;
    private bool isCrouched;
    private float lastJumpTime;
    private float lastDashTime;
    private float lastRewindTime;
    private float lastGroundTime;

    private float wStrafe = 0f;
    private float sStrafe = 0f;
    private float aStrafe = 0f;
    private float dStrafe = 0f;
    private bool jump = false;
    private bool dash = false;
    private bool rewind = false;

    private float timeToRewind;
    Vector3 positionToRewind;

    //Animator animator;
    AudioSource movementSource;

    #endregion private properties

    #region math properties

    float wishSpeed; // speed player desires
    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public Vector3 moveDirectionNormalized;
    private Vector3 playerVelocity;
    Vector3 wishDirection; // direction player desires
    Vector3 frictionVector;

    float addSpeed;
    float accelSpeed;
    float currentSpeed;
    float zspeed;
    float speed;
    float dot;
    float k;
    float accel;
    float newSpeed;
    float control;
    float drop;
    float dashSpeed;

    #endregion math properties

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //animator = GetComponentInChildren<Animator>();
        movementSource = GetComponent<AudioSource>();
        movementSource.playOnAwake = false;
        movementSource.spatialBlend = 1f;
        movementSource.volume = 0.5f;

        timeToRewind = Time.time;
        positionToRewind = transform.position;
    }

    void FixedUpdate()
    {
        GroundCheck();

        if (isGrounded)
        {
            GroundMove((wStrafe + sStrafe), (aStrafe + dStrafe), jump);

            if (!movementSource.isPlaying && (wStrafe + sStrafe != 0 || aStrafe + dStrafe != 0))
            {
                if (isCrouched)
                {
                    Config.enemyAggroRadiusModifier = 0.7f; // enemies cannot detect as far
                    // crouch audio, slower and quieter
                }
                else
                {
                    Config.enemyAggroRadiusModifier = 1f; // enemies can detect normally
                    // walk audio
                }
            }
        }
        else
        {
            AirMove((wStrafe + sStrafe), (aStrafe + dStrafe));
        }
        
        controller.Move(playerVelocity * Time.deltaTime);

        if (Time.time > timeToRewind + Config.rewindAmount)
        {
            timeToRewind = Time.time;
            positionToRewind = transform.position;
        }

        if (rewind)
        {
            transform.position = positionToRewind;
            rewind = false;
        }

        // set animator based on which strafe direction is being applied
        /*if (wStrafe > sStrafe)
        {
            animator.SetInteger("WSMove", 1);
        }
        else if (wStrafe == sStrafe)
        {
            animator.SetInteger("WSMove", 0);
        }
        else if (wStrafe < sStrafe)
        {
            animator.SetInteger("WSMove", -1);
        }
        
        if (aStrafe > dStrafe)
        {
            animator.SetInteger("ADMove", -1);
        }
        else if (aStrafe == dStrafe)
        {
            animator.SetInteger("ADMove", 0);
        }
        else if (aStrafe < dStrafe)
        {
            animator.SetInteger("ADMove", 1);
        }*/
    }

    #region movement methods

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
        lastGroundTime = isGrounded ? Time.time : lastGroundTime;
    }

    void GroundMove(float wsMove, float adMove, bool jump)
    {
        if (jump)
        {
            ApplyFriction(0f); // no friction in air
        }
        else
        {
            ApplyFriction(1f); // ground has friction
        }

        wishDirection = new Vector3(adMove, 0, wsMove); // direction inputs want to go in
        wishDirection = transform.TransformDirection(wishDirection); // get direction in world space
        wishDirection.Normalize();
        moveDirectionNormalized = wishDirection; // get normalized direction

        wishSpeed = wishDirection.magnitude * GetSpeed();

        Accelerate(wishDirection, wishSpeed, GetAcceleration());

        if (jump)
        {
            Jump();
        }
    }

    void AirMove(float wsMove, float adMove)
    {
        wishDirection = new Vector3(adMove, 0, wsMove); // direction inputs want to go in
        wishDirection = transform.TransformDirection(wishDirection); // get direction in world space
        wishDirection.Normalize();
        moveDirectionNormalized = wishDirection; // get normalized direction

        wishSpeed = wishDirection.magnitude * GetSpeed();

        Accelerate(wishDirection, wishSpeed, GetAcceleration());

        ApplyGravity();
    }

    void Accelerate(Vector3 wishDirection, float wishSpeed, float accel)
    {
        currentSpeed = Vector3.Dot(playerVelocity, wishDirection); // source magic
        addSpeed = wishSpeed - currentSpeed; // how much speed is needed
        dashSpeed = 0f; // extra speed to add from dash

        if (addSpeed < 0) return;

        accelSpeed = accel * wishSpeed * Time.deltaTime;

        if (accelSpeed > addSpeed) accelSpeed = addSpeed; // prevent over acceleration

        if (dash)
        {
            dashSpeed = Config.dashSpeed;
            dash = false;
        }

        playerVelocity.x += (accelSpeed + dashSpeed) * wishDirection.x;
        playerVelocity.z += (accelSpeed + dashSpeed) * wishDirection.z;
    }

    void ApplyFriction(float amount)
    {
        frictionVector = playerVelocity;
        frictionVector.y = 0f; // y velocity is not relevant for friction
        speed = frictionVector.magnitude;
        drop = 0f;

        if (isGrounded)
        {
            control = speed < GetDeceleration() ? GetAcceleration() : speed; // if we are slower than the amount we need to slow, speed up
            drop = control * GetFriction() * amount * Time.deltaTime; // how much slow is needed
        }

        newSpeed = speed - drop;
        if (newSpeed < 0) // no negative speed
        {
            newSpeed = 0;
        }
        if (speed > 0)
        {
            newSpeed /= speed; // if player is moving, want player speed to be reduced by the factor of (speed after friction)/(current speed)
        }

        playerVelocity.x *= newSpeed;
        if (!isGrounded) playerVelocity.y *= newSpeed;
        playerVelocity.z *= newSpeed;
    }

    void ApplyGravity()
    {
        playerVelocity.y += Config.gravity * Time.deltaTime;
    }

    void Jump()
    {
        if (isGrounded || ((Time.time - lastGroundTime <= Config.jumpWindow) && lastGroundTime > lastJumpTime)) // is player on ground or within jump window
        {
            playerVelocity.y = Config.jumpSpeed;
            lastJumpTime = Time.time;
            jump = false; // no longer want to jump
            //animator.SetTrigger("Jump");
        }
    }

    float GetSpeed()
    {
        if (isGrounded)
        {
            if (isCrouched)
            {
                return Config.crouchSpeed;
            }
            else
            {
                return Config.sprintSpeed;
            }
        }
        else
        {
            return 1.0f;
        }
    }

    float GetAcceleration()
    {
        if (isGrounded)
        {
            if (isCrouched)
            {
                return Config.crouchAcceleration;
            }
            else
            {
                return Config.acceleration;
            }
        }
        else
        {
            return Config.airAcceleration;
        }
    }

    float GetDeceleration()
    {
        return Config.deceleration;
    }

    float GetFriction()
    {
        if (isGrounded)
        {
            return Config.friction;
        }
        else
        {
            return Config.airFriction;
        }
    }

    #endregion movement methods

    #region input functions

    public void StrafeLeft(InputAction.CallbackContext con)
	{
        aStrafe = con.ReadValue<float>() * -1;
	}

	public void StrafeRight(InputAction.CallbackContext con)
	{
        dStrafe = con.ReadValue<float>();
	}

	public void StrafeForward(InputAction.CallbackContext con)
	{
		wStrafe = con.ReadValue<float>();
	}

	public void StrafeBackward(InputAction.CallbackContext con)
	{
		sStrafe = con.ReadValue<float>() * -1;
	}

	public void Jump(InputAction.CallbackContext con)
	{
		if (con.performed)
		{
			jump = true;
		}
	}

    public void Dash(InputAction.CallbackContext con)
	{
		if (con.performed && Time.time - lastDashTime > Config.dashCooldown)
		{
            dash = true;
            lastDashTime = Time.time;
		}
	}

    public void Crouch(InputAction.CallbackContext con)
    {
        isCrouched = con.ReadValue<float>() > 0.5f;
    }

    public void RewindAbility(InputAction.CallbackContext con)
    {
        if (con.performed && Time.time - lastRewindTime > Config.rewindCooldown)
        {
            rewind = true;
            lastRewindTime = Time.time;
        }
    }

    #endregion input functions
}
