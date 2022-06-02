using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rbody; // Appliceras p� PlayerGameObject i inspektorn   

    // ----------- Variabler f�r Character Horizonal Movement (X-axis) ---------
    private float moveX;
    [Header("Movement Settings:")] // Skapar en header i inspektorn f�r publika inst�llningar
    [Range(0f, 20f)] // Slider f�r att enklare �ndra karakt�rens hastighet i inspektorn
    public float moveSpeed = 5.5f; // R�relsehastigheten, g�r att �ndra i inspektorn

    // ------- Variabler f�r Character Jump (Y-axis) --------------------
    [Range(0f, 20f)] // Skapar en slider f�r hopp-styrkan f�r att enklare �ndra i inspektorn
    public float jumpForce = 10f;

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
    }
    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        CrispJump();
    }

    private void CrispJump()
    {
        if (rbody.velocity.y <0)
        {
            rbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rbody.velocity.y > 0)
        {
            rbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }
    // Anv�nder Unitys nya inputsystem med Unity Events. InputActions kan �ndras/l�ggas till genom InputAction-asset.
    // InputAction Component l�ggs p� PlayerObject. 
    public void Move (InputAction.CallbackContext context)
    {
        moveX = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            PlayerJump();
        }        
    }

    private void PlayerJump()
    {
        rbody.velocity = new Vector2(rbody.velocity.x * 2, jumpForce);
    }
    
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    private void PlayerMove()
    {
        rbody.velocity = new Vector2 (moveX * moveSpeed, rbody.velocity.y);
    }
}
