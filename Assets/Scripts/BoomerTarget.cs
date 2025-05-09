using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerTarget : MonoBehaviour
{
    public Interactible interactible;

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
            Destroy(gameObject);
        }
    }
}
