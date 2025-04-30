using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : Enemy
{

    public float dangerTime;
    public float rotateSpeed;
    private bool timeIsUp = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine(AttackTimer());

        if (timeIsUp == true)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(dangerTime);
        timeIsUp = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            other.GetComponent<HeroController>().health = other.GetComponent<HeroController>().health - 2;
        }
    }
}
