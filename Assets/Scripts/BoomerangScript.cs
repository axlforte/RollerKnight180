using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoomerangScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("yeet");
        if (other.GetComponent<HeroController>())
        {
            print("i should be gone");
            other.GetComponent<HeroController>().gotBoomer = true;
            Destroy(gameObject);
            
        }
    }
}
