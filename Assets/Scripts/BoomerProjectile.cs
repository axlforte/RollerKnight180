using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerProjectile : MonoBehaviour
{
    public float waitTime;
    public bool rotateTowardsOwner = false;
    public GameObject owner = null, grabbedObject = null, RotTowardsTarget;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(rotate()); 
        StartCoroutine(die());
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, 0.5f));
        if (rotateTowardsOwner)
        {
            RotTowardsTarget.transform.LookAt(owner.transform);
            RotateSlightlyTowardsTarget();
        }
        if (grabbedObject != null)
        {
            grabbedObject.transform.position = transform.position;
        }
    }

    void RotateSlightlyTowardsTarget()
    {
        Vector3 Slerp = Vector3.Lerp(transform.rotation.eulerAngles, RotTowardsTarget.transform.rotation.eulerAngles, 0.25f);
        Slerp = new Vector3(Slerp.x, Slerp.y, Slerp.z);

        transform.rotation = Quaternion.Euler(Slerp);
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
        yield return new WaitForSeconds(0.5f);
        rotateTowardsOwner = true;
        owner = GameObject.Find("PlayerHandler");
    }

}
