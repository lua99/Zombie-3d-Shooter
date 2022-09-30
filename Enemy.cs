using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //health


    //enemystuff

    public NavMeshAgent enemyAgent;
    public Transform lookPoint;
    public Camera shootingReycastArea;

    public Transform playerBody;
    public GameObject[] walkPoints;
    int currentEnemyPos = 0;
    public float enemySpeed;
    float walkingPointRadius = 2f;

    public LayerMask playerLayer;



    //sounds and ui

    // enemyshoot

    public float timeBetweenShoot;
    bool previousShot;


    //animations


    //situations

    public float visionRadius;

    public float shootingRadius;

    public bool playerInVisionRadius;
    public bool playerInShootingRadius;

    private void Awake()
    {
        playerBody = GameObject.Find("Player").transform;
        enemyAgent.GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        playerInVisionRadius = Physics.CheckSphere(transform.position,visionRadius,playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

        if(!playerInVisionRadius && !playerInShootingRadius)
        {
            Guard();
        }
        if(playerInVisionRadius && !playerInShootingRadius)
        {
            PursuepLayer();
        }

        if(playerInVisionRadius && playerInShootingRadius)
        {
            ShootPlayer();
        }
    }

    private void Guard()
    {
        if(Vector3.Distance(walkPoints[currentEnemyPos].transform.position,transform.position) < walkingPointRadius)
        {
            currentEnemyPos = Random.Range(0, walkPoints.Length);
            if(currentEnemyPos >= walkPoints.Length)
            {
                currentEnemyPos = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentEnemyPos].transform.position, Time.deltaTime * enemySpeed);

        //changing enemy facing
        transform.LookAt(walkPoints[currentEnemyPos].transform.position);

    }

    void PursuepLayer()
    {
        if (enemyAgent.SetDestination(playerBody.position))
        {
            //+vision and shooting
            visionRadius = 80f;
            shootingRadius = 25f;
        }

    }

    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(lookPoint);

        if (previousShot)
        {
            RaycastHit hit;
            if (Physics.Raycast(shootingReycastArea.transform.position, shootingReycastArea.transform.forward, out hit, shootingRadius))
            {
                Debug.Log("Shooting" + hit.transform.name);
            }

            previousShot = true;
            Invoke(nameof(ActiveShooting), timeBetweenShoot);
        }
    }

    private void ActiveShooting()
    {
        previousShot = false;
    }
}
