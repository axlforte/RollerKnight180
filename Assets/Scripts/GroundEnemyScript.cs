using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 Alexander Lara
 5/10/25
 Spawns an attack every second, dies if health drops to zero
 */

public class GroundEnemyScript : Enemy
{

    public float timeBetweenAttacks;
    public float beginDelay;
    public bool iAmInvincible;
    public bool isBoss;

    public GameObject attackPrefab;

    private HeroController playerReference;

    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        playerReference = GameObject.FindObjectOfType<HeroController>();
        InvokeRepeating("SpawnAttack", beginDelay, timeBetweenAttacks);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && isBoss == true)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("FinishScreen");
        }
        else if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// spawns and attack object on the enemies transform
    /// </summary>
    public void SpawnAttack()
    {
        GameObject projectile = Instantiate(attackPrefab, transform.position, attackPrefab.transform.rotation);
    }


    //checks if the layer object and if six or seven, remove health and play invincibiliy timer
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && iAmInvincible == false)
        {
            health = health - playerReference.swordPower;
            StartCoroutine(BasicHit());
        }
        else if (other.gameObject.layer == 7 && iAmInvincible == false)
        {
            health--;
            StartCoroutine(BasicHit());
        }
    }

    //sets iAmInvincible to true then waits a set time before setting it back to false
    IEnumerator BasicHit()
    {
        iAmInvincible = true;
        Instantiate(hitEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(invTime);
        iAmInvincible = false;
    }

}
