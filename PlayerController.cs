using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;
    private int count;

    // Movement along X and Y axes. 
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    // Controls how high the player jumps
    public float jumpForce = 5.0f;

    // Keeps track of whether the player is on the ground
    public bool isGrounded = true; 

    private int jumpCount = 0;
    public int maxJumps = 2;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player
        rb = GetComponent <Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    // This function is called when a move input is detected.
    void OnMove (InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>(); 

        // Store the X and Y components of the movement. 
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 21) 
        {
            winTextObject.SetActive(true);
        }
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

        // Apply force to the Ridigbody to move the player.s
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void Update()
    {
        // Check for spacebar press and if player is on the ground
        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);  // cancel vertical velocity so double jump doesn't get cancled by gravity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
            // isGrounded = false;  // Player is mid-air after jump
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }

}
