using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    public Player playerScript;
    
    //rifflethings
    public float giveDamageof = 10f;
    public float shootingRange = 100f;

    public float fireCharge = 15f;

    private float nextTimetoShoot = 0f;

    public Animator animator;

    private int maxAmmo = 20;
    private int mag = 15;
    private int presentAmmo;
    public float reloadingTime = 1.3f;
    private bool setRealoading = false;
        
       

    public Camera cam;


    public ParticleSystem muzzleSpark;

    public GameObject impactEffect;


    private void Awake()
    {
        presentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (setRealoading)
            return;

        if (presentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        
        if (Input.GetButton("Fire1") && Time.time >=nextTimetoShoot)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
                
            nextTimetoShoot = Time.time + 1f / fireCharge;
            Shoot();
        }
        else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Idleaim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }


        else if(Input.GetButton("Fire2") && Input.GetButton("Fire1"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Idleaim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }

        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
            animator.SetBool("Reloading", false);


        }
    }

    void Shoot()
    {
        if (mag == 0)
        {
            //text ammo
            return;
        }

        presentAmmo--;
        if (presentAmmo == 0)
        {
            mag--;
        }
        
        
        muzzleSpark.Play();
        
        RaycastHit hitInfo;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            Objects objects = hitInfo.transform.GetComponent<Objects>();

            if(objects != null)
            {
                objects.ObjectHealthDamage(giveDamageof);

                
            }

            GameObject impactGo = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactGo, 1f);
        }
    }

    IEnumerator Reload()
    {
        playerScript.playerSpeed = 0f;
        playerScript.playerSprint = 0f;
        setRealoading = true;
        Debug.Log("Reloading");
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmo = maxAmmo;
        playerScript.playerSpeed = 1.9f;
        playerScript.playerSprint = 3f;
        setRealoading = false;


    }
}
