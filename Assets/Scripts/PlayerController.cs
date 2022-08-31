using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool gameOver = false, disableMovement = false;

    private Animator playerAnim;
    private AudioSource playerAudio;
    public AudioClip jumpSound;

    private Rigidbody rbody; // Appliceras p� PlayerGameObject i inspektorn   

    // ----------- Variabler f�r Character Horizonal Movement (X-axis) ---------
    [Header("Movement Settings:")] // Skapar en header i inspektorn f�r publika inst�llningar
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

    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();

        //Get the Animator component from the Player object that PlayerController is attached to, and store it in the variable playerAnim
        playerAnim = GetComponent<Animator>();
        //Get the Audio source compenent and store it in the playerAudio variable
        playerAudio = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !disableMovement && IsGrounded() && !gameOver)
        {
            Jump();
        }

        if (!disableMovement)
        {
            Move();
        }
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
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), rbody.velocity.y);
        rbody.velocity = new Vector3(input.x * moveSpeed, input.y);
    }
}
