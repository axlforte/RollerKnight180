using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public float invTime = 1;// the invuln needs to be just long enough to not get double hit by a sword swing
    public int damage = 1;//how many quarters of a heart the enemy removes with contact
    public Interactible interactible;//only for enemies that contribute to a counter based system. 

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            health--;
            if(interactible != null)
            {
                interactible.pinged = true;
            }
        }
    }
}
