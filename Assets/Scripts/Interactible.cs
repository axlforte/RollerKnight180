using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 By Davis
 4/28/25
 base class for interactible objects. is a one shot interactible. 
 */

public class Interactible : MonoBehaviour
{
    //this class does no processing. classes that inherit Interactible do have processing that use the bool pinged.
    public bool pinged;
    public bool CanBePingedByPlayer;
}
