using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public CharacterController cc;

    public float playerSpeed = 1.9f;
    public float playerSprint = 3f;

    float vertical;
    float horizontal;

    public Transform playerCamera;

    public float turnCalcTime=0.1f;
    float turnCalcVelocity;


    public Transform surfaceCheck;
    bool onsurface;

    public LayerMask surfaceMask;
    public float surfaceDistance=0.4f;

    Vector3 velocity;

    public float jumpRange = 1f;

    public float gravity = -9.8f;

    public Animator animator;
        
        

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        onsurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if(onsurface=true && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        PlayerMove();
        Jump();
        Sprinting();
    }


    void PlayerMove()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        if (dir.magnitude >= 0.1f)
        {

            animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump");
            animator.SetBool("AimWalk", false);
            animator.SetBool("Idleaim", false);

            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalcVelocity, turnCalcTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cc.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetTrigger("Jump");
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            animator.SetBool("AimWalk", false);

        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && onsurface)
        {
            animator.SetBool("Walk", false);
            animator.SetTrigger("Jump");


            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.ResetTrigger("Jump");
        }
    }

    void Sprinting()

    {

        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onsurface)
        {
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");

            Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

            if (dir.magnitude >= 0.1f)
            {

                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("Idleaim", false);



                float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalcVelocity, turnCalcTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cc.Move(moveDir.normalized * playerSprint * Time.deltaTime);
            }

            else
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
            }
        }
    }
}
