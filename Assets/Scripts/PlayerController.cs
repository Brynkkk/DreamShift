using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5f; 
    public float gravityScale = 1f; 
    public float shakeMagnitude = 1f; 

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
        spawnPoint = transform.position; 
        impulseSource = GameObject.Find("CameraShakeSource").GetComponent<CinemachineImpulseSource>();
        dreamRealityToggle = FindObjectOfType<DreamRealityToggle>(); 

        rb.gravityScale = gravityScale;
        rb.drag = 0; 
        rb.interpolation = RigidbodyInterpolation2D.None; 
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
        float move = Input.GetAxis("Horizontal"); 
        if (move == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y); 
        }
        else
        {
            rb.velocity = new Vector2(move * playerSpeed, rb.velocity.y); 
        }

        if (move > 0)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else if (move < 0)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
    }

    void HandleGravityFlip()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            isGravityFlipped = !isGravityFlipped; // Toggle gravity direction
            rb.gravityScale = isGravityFlipped ? -gravityScale : gravityScale; // Set gravity scale

            // Flip the player's sprite upside down
            transform.localScale = new Vector3(transform.localScale.x, isGravityFlipped ? -1 : 1, transform.localScale.z);
        }
    }

    void HandleDreamworldShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0)) 
        {
            dreamRealityToggle.ToggleState(); // Toggle dream/reality state
        }
    }

    void UpdateAnimation()
    {
        float move = Input.GetAxis("Horizontal");
        bool isWalking = Mathf.Abs(move) > 0.1f;
        animator.SetBool("Walk", isWalking); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle")) 
        {
            transform.position = spawnPoint; // Teleport back to the start
            if (!dreamRealityToggle.IsInReality()) 
            {
                dreamRealityToggle.SetRealityState(); 
            }
        }
        else if (other.CompareTag("Ground"))
        {
            isGrounded = true; 
        }
        else if (other.CompareTag("Checkpoint")) 
        {
            spawnPoint = other.transform.position; // Update spawn point
        }
    }
}
