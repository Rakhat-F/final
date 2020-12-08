using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int temp = 0;
    //Assignables
    public Transform playerCam;
    public Transform orientation;
    public Transform gravity_orientation;
    public LayerMask whatIsWallkable;
    public Transform player;
    
    //RB
    private Rigidbody rb;
    //Movement
    public float gravity = 0f;
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;

    //Rotation and look
    private float xRotation;
    public float sensitivity = 50f;
    private float sensMultiplier = 1f;
    private float maxX = 90f;
    private float minX = -90f;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //WallWalking
    private bool isWallNear = false;
    private bool wallWalking = false;

    //Input
    float x, y;
    bool jumping;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        CheckGround();
        Movement();
    }
    // Update is called once per frame
    void Update()
    {
        MyInput();
        Look();
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
    }

    private void Movement()
    {
        //Extra gravity
        rb.AddForce(-player.transform.up * Time.deltaTime * gravity);

        //Find actual velocity relative to where player is looking
        CounterMovement(rb.velocity);
        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed = this.maxSpeed;


        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > maxSpeed) x = 0;
        if (x < -maxSpeed) x = 0;
        if (y > maxSpeed) y = 0;
        if (y < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded && !wallWalking)
        {
            multiplier = 0f;
            multiplierV = 0f;
        }

        //Apply forces to move player

        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minX, maxX);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);


    }


    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void CounterMovement(Vector3 velocity)
    {
        if (!grounded || jumping) return;

        //Counter movement
        rb.AddForce(-rb.velocity * Time.deltaTime * counterMovement);

    }
    private bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>

    private void CheckGround()
    {
        if (Physics.Raycast(gravity_orientation.position, -gravity_orientation.up, 2, whatIsWallkable))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

}
