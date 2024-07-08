using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5f; // Player speed, editable in the editor
    public float gravityScale = 1f; // Gravity scale when flipping, editable in the editor
    public float shakeMagnitude = 1f; // Screen shake magnitude, editable in the editor

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = true;
    private bool isGravityFlipped = false;
    private Vector3 spawnPoint;
    private CinemachineImpulseSource impulseSource;
    private DreamRealityToggle dreamRealityToggle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position; // Set initial spawn point as the checkpoint
        impulseSource = GameObject.Find("CameraShakeSource").GetComponent<CinemachineImpulseSource>();
        dreamRealityToggle = FindObjectOfType<DreamRealityToggle>(); // Get the DreamRealityToggle script

        rb.gravityScale = gravityScale;
        rb.drag = 0; // Ensure no drag
        rb.interpolation = RigidbodyInterpolation2D.None; // Ensure no interpolation
    }

    void Update()
    {
        MovePlayer();
        HandleGravityFlip();
        HandleDreamworldShift();
        UpdateAnimation();
    }

    void MovePlayer()
    {
        float move = Input.GetAxis("Horizontal"); // Get input from A|D or arrow keys
        if (move == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Stop movement when no input
        }
        else
        {
            rb.velocity = new Vector2(move * playerSpeed, rb.velocity.y); // Move the player
        }

        // Flip the player's sprite based on movement direction
        if (move > 0)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else if (move < 0)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
    }

    void HandleGravityFlip()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Check if space key is pressed
        {
            isGravityFlipped = !isGravityFlipped; // Toggle gravity direction
            rb.gravityScale = isGravityFlipped ? -gravityScale : gravityScale; // Set gravity scale

            // Flip the player's sprite upside down
            transform.localScale = new Vector3(transform.localScale.x, isGravityFlipped ? -1 : 1, transform.localScale.z);
        }
    }

    void HandleDreamworldShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0)) // Check if shift or left mouse button is pressed
        {
            impulseSource.GenerateImpulse(shakeMagnitude); // Trigger screen shake
            dreamRealityToggle.ToggleState(); // Toggle dream/reality state
        }
    }

    void UpdateAnimation()
    {
        float move = Input.GetAxis("Horizontal");
        bool isWalking = Mathf.Abs(move) > 0.1f;
        animator.SetBool("Walk", isWalking); // Set the Walk bool in the animator
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle")) // Check if collided with an obstacle
        {
            transform.position = spawnPoint; // Teleport back to the start
            if (!dreamRealityToggle.IsInReality()) // Ensure the state is reality
            {
                dreamRealityToggle.SetRealityState(); // Change to reality state
            }
        }
        else if (other.CompareTag("Ground"))
        {
            isGrounded = true; // Set grounded state
        }
        else if (other.CompareTag("Checkpoint")) // Check if collided with a checkpoint
        {
            spawnPoint = other.transform.position; // Update spawn point
        }
    }
}
