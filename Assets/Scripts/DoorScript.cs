using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 By Davis
 4/28/25
 Door that opens when you interact while close to it
 */

public class DoorScript : Interactible
{
    public Vector3 bottom, top;
    public Vector3 origin;
    public GameObject DoorCollision;
    public float raiseTime;

    // Start is called before the first frame update
    void Start()
    {
        origin = DoorCollision.transform.position;
        DoorCollision.transform.position = origin + bottom;
    }

    // Update is called once per frame
    void Update()
    {
        if (pinged)
        {
            DoorCollision.transform.position = Vector3.Lerp(DoorCollision.transform.position, origin + top, 1f / raiseTime);
        }
    }
}
