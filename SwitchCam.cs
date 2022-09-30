using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCam : MonoBehaviour
{

    public GameObject aimCam;
    public GameObject aimCanvas;
    public GameObject thirdPersonCam;
    public GameObject thirdPersonCanvas;

    public Animator animator;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if(Input.GetButton("Fire2")&& Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){

            animator.SetBool("Idle", false);
            animator.SetBool("Idleaim", true);
            animator.SetBool("AimWalk", true);
            animator.SetBool("Walk", true);


            thirdPersonCam.SetActive(false);
            thirdPersonCanvas.SetActive(false);
            aimCam.SetActive(true);
            aimCanvas.SetActive(true);


        }
        else if(Input.GetButton("Fire2"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Idleaim", true);
            animator.SetBool("AimWalk", false);
            animator.SetBool("Walk", false);



            thirdPersonCam.SetActive(false);
            thirdPersonCanvas.SetActive(false);
            aimCam.SetActive(true);
            aimCanvas.SetActive(true);
        }

        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Idleaim", false);
            animator.SetBool("AimWalk", false);
           



            thirdPersonCam.SetActive(true);
            thirdPersonCanvas.SetActive(true);
            aimCam.SetActive(false);
            aimCanvas.SetActive(false);
        }
    }
}
