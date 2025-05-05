using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counter : Interactible
{
    public int maxCount, count;
    public Interactible otherInteractible;

    // Update is called once per frame
    void Update()
    {
        if (pinged)
        {
            pinged = false;
            count++;
            if (count >= maxCount)
            {
                otherInteractible.pinged = true;
                Destroy(gameObject);
            }
        }
    }
}
