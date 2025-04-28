using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    //tooltips are cool. dassit
    [Header("Player Movement Data")]
    [Tooltip("The physics layers that the player can interact with")]
    public LayerMask LM;
    public KeyCode Forward, Backward, Left, Right;
    public Rigidbody RB;
    public float speed, jumpForce;
    public GameObject LockOnTarget;
    public bool Aiming;
    public KeyCode Jumping, Item1, Item2, Interact, Sword;
    [Header("Camera Data")]
    public float rotIntensity;
    public KeyCode UpCam, DownCam, LeftCam, RightCam;
    public Transform PlayerModel, cam;
    public Quaternion cameraRot;
    [Header("Item Data")]

    public int swordPower;
    public bool gotBoomer;
    public bool gotBow;
    public int basicKeys;
    public int health;// per quarter heart

    //only if we plan on making arrows limited but I personal dont think so
    //jeans: we should, if people dont like it just disable it lol
    public int arrows;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /* too lazy to do time.deltatime
       so I made the default update into fixed update
       fixedupdate triggers at 50fps afaik, so its always
       being called in a fixed amount of time
    */
    void FixedUpdate()
    {
        //you can only move around when you are grounded
        if (GetGrounded())
        {
            Move();
            Jump();
        }
        PositionCamera();
    }

    private bool GetGrounded()
    {
        //this is broken why 
        //i need to do some fuckery to see whats up
        Debug.Log(Physics.Raycast(RB.transform.position, Vector3.down, 0.01f, LM));
        return Physics.Raycast(RB.transform.position, Vector3.down, 0.01f, LM);
    }

    private void Move()
    {
        //the player strafes if they are locked on, and if they are aiming
        if(LockOnTarget != null)
        {

            Debug.Log("hee hee");
        //the player will not move when aiming. locking on takes priority.
        } else if (Aiming)
        {
            Debug.Log("nop");
        //otherwise you are walking normally. rotate towards movement direction, not target.
        } else
        {
            float rot = Rotation();
            if (rot != -2)
            {
                //RB.AddForce(RB.transform.forward * speed);
                transform.Translate(RB.transform.forward * speed * Time.deltaTime);

                //why unity why
                RB.rotation = Quaternion.Euler(0, ((rot * 180) + cameraRot.eulerAngles.y) / 2, 0);
            }
        }
        //Debug.Log(RB.transform.forward);
    }

    //moves the camera to the player, rotates to its rotation, then moves back a certain amount
    private void PositionCamera()
    {

        //move the camera in relation to 
        cameraRot = Quaternion.Euler(
            cameraRot.eulerAngles.x + (BoolToInt(Input.GetKey(DownCam))) - (BoolToInt(Input.GetKey(UpCam))), 
            cameraRot.eulerAngles.y + (BoolToInt(Input.GetKey(LeftCam))) - (BoolToInt(Input.GetKey(RightCam))), 
            cameraRot.eulerAngles.z);

        cam.position = PlayerModel.position;
        cam.rotation = cameraRot;
        //this lags the fuck out of my computer. optimize or get rid of bloatware i guess

    }

    //returns the rotation of the player movement.
    //so the move function can know if the player is moving in a direction
    private float Rotation()
    {
        /*
         0 = forward
        0.5 = right
        -0.5 = left
        -1 / 1 = backwards
        -2 = nothing pressed
         */

        bool up = Input.GetKey(Forward);
        bool down = Input.GetKey(Backward);
        bool left = Input.GetKey(Left);
        bool right = Input.GetKey(Right);

        if (up)
        {
            if (left && !right)
                return -0.25f;
            if (right && !left)
                return 0.25f;
            if (down)
                return -2;
            return 0;
        } else if (down)
        {
            if (left && !right)
                return -0.75f;
            if (right && !left)
                return 0.75f;
            if (up)
                return -2;
            return 1;//either positive or negative. it shouldnt matter.
        } else if (right)
        {
            return 0.5f;
        } else if (left)
        {
            return -0.5f;
        }


        return -2;
        //the returned value needs to be multiplied by 180, not 360
    }

    private void Jump()
    {
        Debug.Log("i can jump!");
        if (Input.GetKeyDown(Jumping))
        {
            RB.AddForce(Vector3.up * jumpForce);
        }
    }

    //A helper function that converts booleans to integers. I dont know if casting works and i am too lazy to check
    public int BoolToInt(bool boolean)
    {
        if (boolean)
        {
            return 1;
        } else
        {
            return 0;
        }
    }
}
