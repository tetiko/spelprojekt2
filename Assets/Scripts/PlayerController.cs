using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rbody; // Appliceras på PlayerGameObject i inspektorn   

    // ----------- Variabler för Character Horizonal Movement (X-axis) ---------
    private float moveX;
    [Header("Movement Settings:")] // Skapar en header i inspektorn för publika inställningar
    [Range(0f, 20f)] // Slider för att enklare ändra karaktärens hastighet i inspektorn
    public float moveSpeed = 5.5f; // Rörelsehastigheten, går att ändra i inspektorn

    // ------- Variabler för Character Jump (Y-axis) --------------------
    [Range(0f, 20f)] // Skapar en slider för hopp-styrkan för att enklare ändra i inspektorn
    public float jumpForce = 10f;

    // -------------------- Variabler för ett mindre floaty hopp --------------------
    public float fallMultiplier = 1.5f;
    public float lowJumpMultiplier = 2f;

    // ------- Variabler för Ground Checking (så du inte kan hoppa in luften, utan enbart när du är grounded --------------------
    [Header("Settings for Ground Checking")]
    public Transform groundCheck; // Sätts i inspektorn, i detta fall ett tomt GameObject i botten av karaktären
    public LayerMask groundLayer; // Sätts i inspektorn
    public float groundRadius = 0.2f; // Hur stort avståndet är i GroundDetecting

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
    // Använder Unitys nya inputsystem med Unity Events. InputActions kan ändras/läggas till genom InputAction-asset.
    // InputAction Component läggs på PlayerObject. 
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
