using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : Enemy
{

    public float dangerTime;
    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttackTimer());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime,0);
    }

    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(dangerTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>() && other.GetComponent<HeroController>().iAmInvincible == false)
        {
            other.GetComponent<HeroController>().health = other.GetComponent<HeroController>().health - damage;
        }
    }
}
