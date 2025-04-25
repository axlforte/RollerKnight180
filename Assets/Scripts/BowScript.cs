using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            other.GetComponent<HeroController>().gotBow = true;
            Destroy(gameObject);
        }
    }
}
