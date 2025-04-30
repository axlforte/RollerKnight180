using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundEnemyScript : Enemy
{

    public float timeBetweenAttacks;
    public float beginDelay;

    public GameObject attackPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnAttack", beginDelay, timeBetweenAttacks);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnAttack()
    {
        GameObject projectile = Instantiate(attackPrefab, transform.position, attackPrefab.transform.rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            health--;
        }
    }
}
