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

}
