using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubieScript : MonoBehaviour
{
    public int rubieValue;

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
        if (other.GetComponent<HeroController>())
        {
            other.GetComponent<HeroController>().rubies = other.GetComponent<HeroController>().rubies + rubieValue;
            Destroy(gameObject);

        }
    }

}
