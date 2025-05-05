using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerProjectile : MonoBehaviour
{
    public float waitTime;
    public bool rotateTowardsOwner = false;
    public GameObject owner = null, grabbedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(rotate());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, 0.5f));
        if (rotateTowardsOwner)
        {
            transform.LookAt(owner.transform);
        }
        if (grabbedObject != null)
        {
            grabbedObject.transform.position = transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner)
        {
            Destroy(gameObject);
        } else if (grabbedObject == null)
        {
            if (other.GetComponent<KeyScript>())
            {
                grabbedObject = other.gameObject;
            }
            else if (other.GetComponent<RubieScript>())
            {
                grabbedObject = other.gameObject;
            }
        }
    }

    //istg i could just do this with a basic timer but NO we have to have a coroutine!
    IEnumerator rotate()
    {
        rotateTowardsOwner = false;
        yield return new WaitForSeconds(1);
        rotateTowardsOwner = true;
        owner = GameObject.Find("PlayerHandler");
    }

}
