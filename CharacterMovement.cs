using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float wrSpeed = 15f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform gc;

    float gravity = 9.2f;
    public LayerMask ground;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private Rigidbody rb;
    bool isGrounded;

    bool crouched;
    bool jump;
    float excel = 10f;
    float gCS = 0.4f;

    // keeping this because i dont want to rewrite this
    enum Mode
    {
        Running,
    }
    Mode mode = Mode.Running;

    bool canDoubleJump;

    Vector3 dir = Vector3.zero;
    void start()
    {

    }

    void Update()
    {
        jump = Input.GetButtonDown("Jump");
        crouched = Input.GetKey(KeyCode.LeftShift);
        isGrounded = Physics.CheckSphere(gc.position, gCS, ground);
        col.material.dynamicFriction = 0;
        dir = Direction();
        if (crouched)
        {
            runSpeed = 15f;
            rb.drag = 0.25f;
            col.height = 1.8f;
        }
        else
        {
            runSpeed = 12f;
            rb.drag = 0f;
            col.height = 2.5f;
        }
        switch (isGrounded)
        {

            case true:
                canDoubleJump = true;
                break;
        }

        switch (mode)
        {

            case Mode.Running:
                run(dir, runSpeed, excel);
                break;
        }
    }

    void run(Vector3 whichDir, float maxSpeed, float acceleration)
    {
        if (isGrounded && jump)
        {
            Jump();
        }
        else if (canDoubleJump && jump)
        {
            Jump();
            canDoubleJump = false;
        }
        else
        {
            crouched = Input.GetKey(KeyCode.LeftShift);
            whichDir = whichDir.normalized;
            Vector3 speed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (speed.magnitude > maxSpeed) acceleration *= speed.magnitude / maxSpeed;
            Vector3 direction = whichDir * maxSpeed - speed;

            rb.AddForce(direction, ForceMode.Acceleration);
        }
    }

    void Jump()
    {

        float upforce = Mathf.Clamp(gravity - rb.velocity.y, 0, Mathf.Infinity);
        rb.AddForce(new Vector3(0f, upforce, 0f), ForceMode.VelocityChange);
    }



    private Vector3 Direction()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, 0, y);
        return rb.transform.TransformDirection(direction);
    }
}