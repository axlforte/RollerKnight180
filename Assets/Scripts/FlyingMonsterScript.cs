using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingMonsterScript : Enemy
{

    public GameObject wherePlayerIsAt;

    public NavMeshAgent agent;

    public float detectionRange;

    // Start is called before the first frame update
    void Start()
    {
 
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
        if (other.gameObject.layer == 6)
        {
            health--;
        }
        else if (other.gameObject.layer == 7)
        {
            health--;
        }

        if (other.GetComponent<HeroController>())
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
}
