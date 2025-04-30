using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public float invTime = 8;// the invuln needs to be just long enough to not get double hit by a sword swing
    public int damage = 1;//how many quarters of a heart the enemy removes with contact
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
