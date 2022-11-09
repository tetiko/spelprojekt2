using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Access external scripts
    PlayerManager playerManager;

    [HideInInspector] public bool gameOver = false, disableMovement = false, disableJump = false;

    private Animator playerAnim;
    private AudioSource playerAudio;
    public AudioClip jumpSound;

    [HideInInspector] public Rigidbody rbody; // Appliceras p� PlayerGameObject i inspektorn   

    // ----------- Variabler f�r Character Horizonal Movement (X-axis) ---------
    [Header("Movement Settings")] // Skapar en header i inspektorn f�r publika inst�llningar
    public float moveSpeed = 20f; // R�relsehastigheten, g�r att �ndra i inspektorn

    // ------- Variabler f�r Character Jump (Y-axis) --------------------
    public float jumpForce = 80;

    // -------------------- Variabler f�r ett mindre floaty hopp --------------------
    public float fallMultiplier = 1.5f;
    public float lowJumpMultiplier = 2f;

    // ------- Variabler f�r Ground Checking (s� du inte kan hoppa in luften, utan enbart n�r du �r grounded --------------------
    [Header("Settings for Ground Checking")]
    public Transform groundCheck; // S�tts i inspektorn, i detta fall ett tomt GameObject i botten av karakt�ren
    public LayerMask groundLayer; // S�tts i inspektorn
    public float groundRadius = 0.2f; // Hur stort avst�ndet �r i GroundDetecting

    [Header("Default forces applied to player upon taking damage")]
    public float impactForceX;
    public float impactForceY;

    // ------- Variabler f�r hazards och interactables som p�verkar spelarens movement --------------------
    [Header("Hazards and Interactables")]
    public float bounceForceX;
    public float bounceForceY;
    public float bounceControl = 2f;
    public float webbedMoveSpeed = 1f;
    public float oilMoveSpeed = 1.5f;

    public float vel;
    public float inp;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        rbody = GetComponent<Rigidbody>();

        //Get the Animator component from the Player object that PlayerController is attached to, and store it in the variable playerAnim
        playerAnim = GetComponent<Animator>();
        //Get the Audio source compenent and store it in the playerAudio variable
        playerAudio = GetComponent<AudioSource>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !disableMovement && !disableJump && IsGrounded() && !gameOver)
        {
            Jump();
        }

        if (!disableMovement && !playerManager.bouncing)
        {
            Move();
        }
        else if (playerManager.bouncing)
        {
            BounceControl();
        }

        vel = rbody.velocity.x;
        inp = Input.GetAxis("Horizontal");
    }

    private void Jump()
    {
        rbody.velocity = new Vector2(rbody.velocity.x * 2, jumpForce);

        if (rbody.velocity.y < 0)
        {
            rbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rbody.velocity.y > 0)
        {
            rbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //set the trigger 'jump_trig' for the player object's animator

        //playeranim.settrigger("jump_trig");
        //playeraudio.playoneshot(jumpsound, 0.3f);
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    private void Move()
    {
        //If walking on non slippery material
        if (!playerManager.slippery)
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), rbody.velocity.y);
            rbody.velocity = new Vector3(input.x * moveSpeed, input.y);        
        }
        else
        {
            //Stop gliding upon controller input
            if (Input.GetButton("Horizontal"))
            {
                playerManager.glide = false;

                //If player is not gliding, lerp velocity to make the surface seem slippery
                rbody.velocity = new Vector3(Mathf.Lerp(rbody.velocity.x, oilMoveSpeed, Input.GetAxis("Horizontal")), rbody.velocity.y); 
            }
            //Glide if there's no controller input
            else if (!Input.GetButton("Horizontal"))
            {
                playerManager.glide = true;
            }
        }
    }

    private void BounceControl()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal") * bounceControl, 0);
        rbody.AddForce(input, ForceMode.Force);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
