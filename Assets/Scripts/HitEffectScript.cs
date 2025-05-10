using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(die());
    }

    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z) *(1 + Time.deltaTime * 5);
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
