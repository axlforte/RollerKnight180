using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            other.GetComponent<HeroController>().basicKeys ++;
            Destroy(gameObject);
        }
    }
}
