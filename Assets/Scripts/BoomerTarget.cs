using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerTarget : MonoBehaviour
{
    public Interactible interactible;
    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoomerProjectile>())
        {
            interactible.pinged = true;
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
