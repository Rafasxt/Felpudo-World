using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Pulo")]
    public float jumpForce = 7.5f;   

    [Header("Detecção de chão")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Movimento opcional")]
    public float baseMoveSpeed = 0f;   
    private float currentMoveSpeed;
    private float boostTimer = 0f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        else
            isGrounded = true;

        
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isGrounded)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        
        if (boostTimer > 0f)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
                currentMoveSpeed = baseMoveSpeed;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(currentMoveSpeed, rb.linearVelocity.y);
    }

    public void ApplySpeedBoost(float newSpeed, float duration)
    {
        currentMoveSpeed = newSpeed;
        boostTimer = duration;
    }
}
