using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingMonsterScript : Enemy
{

    public GameObject wherePlayerIsAt;

    public NavMeshAgent agent;
 
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        KeepLooking();

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
    }
    
    private void KeepLooking()
    {
        transform.LookAt(wherePlayerIsAt.transform);
        agent.SetDestination(wherePlayerIsAt.transform.position);
    }
}
