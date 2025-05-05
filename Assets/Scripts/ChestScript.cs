using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{

    public GameObject itemInChest;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HeroController>() && itemInChest != null)
        {
            GameObject projectile = Instantiate(itemInChest, transform.position + new Vector3(0,1,0), itemInChest.transform.rotation);
            itemInChest = null;
        }
    }
}
