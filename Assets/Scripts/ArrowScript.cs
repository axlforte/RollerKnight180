using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    // Start is called before the first frame update
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
        transform.Translate(new Vector3(0,0,0.66f));
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
