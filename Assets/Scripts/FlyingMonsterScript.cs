using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 Alexander Lara
 5/10/25
 looks at player and chases, dies when health is zero 
 */

public class FlyingMonsterScript : Enemy
{
    public bool frozen;

    public GameObject wherePlayerIsAt;

    public NavMeshAgent agent;

    public float detectionRange;

    public bool iAmInvincible;

    private HeroController playerReference;

    //davis added this. he thinks that seperating variables like the variables above is infuriating!
    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        playerReference = GameObject.FindObjectOfType<HeroController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen)
        return;

        KeepLooking();
        Detect();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) //sword layer
        {
            health = health - playerReference.swordPower;
            StartCoroutine(BasicHit());
        }
        else if (other.gameObject.layer == 7) //arrow layer
        {
            health--;
        }
        else if (other.gameObject.layer == 8) // boomerang layer
        {
            StartCoroutine(Stunned());
        }

    }

    //always rotate the transform so that it faces the player
    private void Detect()
    {
        if (Vector3.Distance(wherePlayerIsAt.transform.position, transform.position) <= detectionRange)
        {
            agent.SetDestination(wherePlayerIsAt.transform.position);
        }
    }

    private void KeepLooking()
    {
        transform.LookAt(wherePlayerIsAt.transform);
    }

    //sets iAmInvincible to true then waits a set time before setting it back to false
    IEnumerator BasicHit()
    {
        iAmInvincible = true;
        yield return new WaitForSeconds(invTime);
        iAmInvincible = false;
    }

    //freeze script then unfreezes after set time
    IEnumerator Stunned()
    {
        frozen = true;
        Instantiate(hitEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(4);
        frozen = false;
    }
}
