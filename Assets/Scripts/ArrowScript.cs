using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(die());
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0,0,0.33f));
    }
}
