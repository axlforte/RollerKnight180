using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingMonsterScript : Enemy
{

    public GameObject wherePlayerIsAt;

    public NavMeshAgent agent;

    public float detectionRange;

    public bool iAmInvincible;

    private HeroController playerReference;

    // Start is called before the first frame update
    void Start()
    {
        playerReference = GameObject.FindObjectOfType<HeroController>();
    }

    // Update is called once per frame
    void Update()
    {
        KeepLooking();
        Detect();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 )
        {
            health = health - playerReference.swordPower;
            StartCoroutine(BasicHit());
        }
        else if (other.gameObject.layer == 7)
        {
            health--;
        }

        if (other.GetComponent<HeroController>() && other.GetComponent<HeroController>().iAmInvincible == false)
        {
            other.GetComponent<HeroController>().health = other.GetComponent<HeroController>().health - damage;
        }
    }

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

    IEnumerator BasicHit()
    {
        iAmInvincible = true;
        yield return new WaitForSeconds(invTime);
        iAmInvincible = false;
    }
}
