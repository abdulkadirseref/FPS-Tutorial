using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float speed = 8f;
    [SerializeField] float gravity = -30f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float acceleration = 2f;
    [SerializeField] bool isGrounded;
    [SerializeField] float sprintSpeed;

    public Ak ak;

    Vector3 verticalVelocity = Vector3.zero;
    Vector2 horizontalInput;

    bool jump;
    public bool sprint;
    private float currentSpeed = 0f;


    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//


    private void Start()
    {
        controller = GetComponent<CharacterController>();

    }


    void Update()
    {
        Move();
    }


    public void Move()
    {
        float halfHeight = controller.height * 0.5f;
        var bottomPoint = transform.TransformPoint(controller.center - Vector3.up * halfHeight);
        isGrounded = Physics.CheckSphere(bottomPoint, 0.1f, groundLayer);
        if (isGrounded)
        {
            verticalVelocity.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprint = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprint = false;
        }

        // Calculate horizontal velocity based on input and acceleration
        Vector3 desiredVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        if (sprint)
        {
            desiredVelocity *= sprintSpeed; // Apply sprint speed multiplier

        }
        currentSpeed = Mathf.Lerp(currentSpeed, desiredVelocity.magnitude, Time.deltaTime * acceleration);
        Vector3 horizontalVelocity = desiredVelocity.normalized * currentSpeed;



        controller.Move(horizontalVelocity * Time.deltaTime);



        if (ak.isAiming == true)
        {
            speed = 2.5f;
            sprint = false;
        }
        else
        {
            speed = 5f;
        }


        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    public void OnJumpPressed()
    {
        jump = true;
    }
}
