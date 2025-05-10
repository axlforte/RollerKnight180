using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
 Davis Williams
4/30/25
 makes the player perform all needed actions to complete a dungeon with a bow and boomerang. 
 */

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
    public Interactible inter;
    public bool Aiming;
    public KeyCode Jumping, Item1, Item2, Interact, SwingSword, lockOn;
    public GameObject AimingReticle;
    [Header("Camera Data")]

    public float rotIntensity;
    public float camDist;
    public KeyCode UpCam, DownCam, LeftCam, RightCam;
    public Transform PlayerModel, cam, camPivot;
    public Quaternion cameraRot;
    [Header("Item Data")]

    public int swordPower;
    public Transform Sword;
    public Vector3 swingStart, swingEnd;//i only want the one swing, maybe a jump attack for lockon
    [Tooltip("The amount of time the sword swings for, in physics frames (1/50th of a second)")]
    public float swingMaxTime, swingSpeed;
    float swingTime;
    public bool gotBoomer;
    public GameObject boomerangPrefab;
    public bool gotBow;
    public GameObject arrowPrefab;
    public int basicKeys;
    public int rubies;// legally distinct rupees
    public int health;// per quarter heart
    public int maxHealth;

    [Header("UI Data")]
    //public Sprite[] hearts;//the levels of health, from full heart to none;
   // public Image[] UIHearts;//the images on the HUD
    public TMP_Text healthGText;

    public bool iAmInvincible;

    [Header("Input Data")]//this section is going to suck. but I dont want to move away from fixedupdate because it requires shifting my mentality
    // to using Time.deltaTime, which is unrealistic when im involved with another project that has a fixed update cycle.

    //the gist - each input (that needs it) gets a new bool for pressed, held, and last held. 
    // pressed == held && !pressed
    // you can used held != lastHeld and !pressed to check if the key was just released

    public int floof;//just to prevent funky header stuff
    public bool BowPressed, BowHeld, BowLastHeld;
    public bool BoomerPressed, BoomerHeld, BoomerLastHeld;
    public bool JumpPressed, JumpHeld, JumpLastHeld;
    public bool SwordPressed, SwordHeld, SwordLastHeld;
    public bool LockPressed, LockHeld, LockLastHeld;
    public bool InteractPressed, InteractHeld, InteractLastHeld;

    // Start is called before the first frame update
    void Start()
    {
        AimingReticle.SetActive(false);
    }

    /* too lazy to do time.deltatime
       so I made the default update into fixed update
       fixedupdate triggers at 50fps afaik, so its always
       being called in a fixed amount of time
    */
    void FixedUpdate()
    {
        //get the current state of the keys. only keys that I need to check are pressed/released, not just held
        UpdateKeys();

        //you can only move around when you are grounded
        if (GetGrounded())
        {
            Jump();
        }
        //if you want to lock on, find the nearest visible enemy and set the lock on target to that
        LockOntoEnemy();
        //if you are pressing a move key, apply movement
        Move();
        //check if you are in an interactible. if so, check if you can interact when you interact with it
        Interaction();
        //move the camera to the appropriate position, also checks for collision, to prevent the camera from clipping out of bounds
        PositionCamera();
        //check if the player wants to swing, and if so start swinging.
        swordSwing();
        //this doesnt need to be a function anymore. all it does is set a string to the health value.
        UpdateHearts();
        //if you have the bow, check for bow things
        if (gotBow)
        {
            Bow();
        }
        //if you have the boomerang, check for boomerang things
        if(gotBoomer)
        {
            Boomerang();
        }
    }

    //gets if the player is currently colliding with the ground
    private bool GetGrounded()
    {
        return Physics.Raycast(RB.transform.position + new Vector3(0,1,0), Vector3.down, 1.1f, LM);
    }

    //moves the player in relation to their current rotation
    private void Move()
    {
        //the player strafes if they are locked on, and if they are aiming
        if(LockOnTarget != null)
        {
            float rot = Rotation();
            if (rot != -2)
            {
                //RB.AddForce(RB.transform.forward * speed);
                transform.LookAt(LockOnTarget.transform);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                transform.Translate(new Vector3(Mathf.Sin(rot * 180),0, Mathf.Cos(rot * 180)) * speed * Time.deltaTime);

                //why unity why
                //the value is divided by 2 because it makes the processes work properly
                //RB.rotation = Quaternion.Euler(0, ((rot * 180) + cameraRot.eulerAngles.y) / 2, 0);
                //PlayerModel.localRotation = RB.rotation;
            }
            //the player will not move when aiming. locking on takes priority.
        } else if (Aiming) {
            RB.rotation = Quaternion.Euler(0, cam.rotation.eulerAngles.y, 0);
        //otherwise you are walking normally. rotate towards movement direction, not target.
        } else
        {
            float rot = Rotation();
            if (rot != -2)
            {
                //RB.AddForce(RB.transform.forward * speed);
                transform.Translate(RB.transform.forward * speed * Time.deltaTime);

                //why unity why
                //the value is divided by 2 because it makes the processes work properly
                RB.rotation = Quaternion.Euler(0, ((rot * 180) + cameraRot.eulerAngles.y) / 2, 0);
                PlayerModel.localRotation = RB.rotation;
            }
        }
        //Debug.Log(RB.transform.forward);
    }

    //moves the camera to the player, rotates to its rotation, then moves back a certain amount
    private void PositionCamera()
    {
        if (LockOnTarget == null)
        {
            bool up = false;
            bool down = false;
            bool left = Input.GetKey(LeftCam);
            bool right = Input.GetKey(RightCam);

            if (cameraRot.eulerAngles.x < 89 || cameraRot.eulerAngles.x > 271)
            {
                up = Input.GetKey(UpCam);
                down = Input.GetKey(DownCam);
            }
            else
            {
                if (cameraRot.eulerAngles.x > 180)
                {
                    cameraRot = Quaternion.Euler(
                        cameraRot.eulerAngles.x + 1,
                        cameraRot.eulerAngles.y,
                        cameraRot.eulerAngles.z);
                }
                else
                {
                    cameraRot = Quaternion.Euler(
                        cameraRot.eulerAngles.x - 1,
                        cameraRot.eulerAngles.y,
                        cameraRot.eulerAngles.z);
                }
            }

           // Debug.Log(cameraRot.eulerAngles.x);

            //move the camera in relation to 
            cameraRot = Quaternion.Euler(
                cameraRot.eulerAngles.x + ((BoolToInt(down)) - (BoolToInt(up))) * rotIntensity,
                cameraRot.eulerAngles.y + ((BoolToInt(right)) - (BoolToInt(left))) * rotIntensity,
                cameraRot.eulerAngles.z);

            camPivot.position = PlayerModel.position + Vector3.up * 0.5f;
            camPivot.rotation = cameraRot;
            if (Aiming)
            {
                cam.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                //get the distance the camera can comfortably move without being in a wall, then remove 0.1
                RaycastHit hit;
                if (Physics.Raycast(camPivot.position, camPivot.forward * -1, out hit, camDist))
                    cam.localPosition = new Vector3(0, 0, (hit.distance - 0.1f) * -1);
                else 
                    cam.localPosition = new Vector3(0, 0, -camDist);
            }

            //this lags the fuck out of my computer. optimize or get rid of bloatware i guess
        } else
        {   //and from this day forth, HeroController was known as pythor the undebuggable.
            camPivot.position = new Vector3((RB.transform.position.x + LockOnTarget.transform.position.x) / 2,
                (RB.transform.position.y + LockOnTarget.transform.position.y) / 2,
                (RB.transform.position.z + LockOnTarget.transform.position.z) / 2);
            camPivot.rotation = Quaternion.Euler(30, RB.transform.rotation.eulerAngles.y + 90,0);
            cam.localPosition = new Vector3(0, 0, Vector3.Distance(RB.transform.position, LockOnTarget.transform.position) * -0.5f - 4);
        }
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
        if (LockOnTarget != null)
        {
            right = Input.GetKey(Forward);
            left = Input.GetKey(Backward);
            up = Input.GetKey(Left);
            down = Input.GetKey(Right);
        }

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

    //if the player is grounded, jump
    private void Jump()
    {
        //Debug.Log("i can jump!");
        if (JumpPressed)
        {
           // Debug.Log("jumped!");
            RB.AddForce(Vector3.up * jumpForce);
        }
    }

    //activates the sword hitbox, moves it in a horiz swipe, then disables the hitbox
    private void swordSwing()
    {

        if (swingTime >= 0.5f)
        {
            swingTime = swingTime / swingSpeed;
            Sword.localRotation = Quaternion.Slerp(Quaternion.Euler(swingStart.x, swingStart.y, swingStart.z), Quaternion.Euler(swingEnd.x, swingEnd.y, swingEnd.z), 1 - (swingTime / swingMaxTime));
        }
        else if (swingTime >= 0)
        {
            Sword.gameObject.SetActive(false);
            swingTime--;
        }
        else if (SwordPressed)//istg i tried to make these readable
        {
            //Debug.Log("swing!");
            swingTime = swingMaxTime;
            Sword.gameObject.SetActive(true);
            Sword.localRotation = Quaternion.Slerp(Quaternion.Euler(swingStart.x, swingStart.y, swingStart.z), Quaternion.Euler(swingEnd.x, swingEnd.y, swingEnd.z), 1 - (swingTime / swingMaxTime));
        }
    }

    //item1 is bow
    private void Bow()
    {
        if (BowPressed)
        {
            Aiming = true;
            AimingReticle.SetActive(true);
        }
        else if (BowHeld != BowLastHeld)
        {
            Aiming = false;
            AimingReticle.SetActive(false);
            if (LockOnTarget != null)
                Instantiate(arrowPrefab, transform.position + Vector3.up, PlayerModel.transform.rotation);
            else
                Instantiate(arrowPrefab, transform.position + Vector3.up, cam.transform.rotation);
        }
    }

    //item2 is boomerang
    private void Boomerang()
    {
        if (BoomerPressed)
        {
            Aiming = true;
            AimingReticle.SetActive(true);
        }
        else if (BoomerHeld != BoomerLastHeld)
        {
            Aiming = false;
            AimingReticle.SetActive(false);
            if (LockOnTarget != null)
                Instantiate(boomerangPrefab, transform.position + Vector3.up * 2, PlayerModel.transform.rotation);
            else
                Instantiate(boomerangPrefab, transform.position + Vector3.up * 2, cam.transform.rotation);
        }
    }

    //interact with the currently availible interactible
    //current method is ass, and does not work if you are in multiple interactible zones
    private void Interaction()
    {
        if(inter != null && InteractPressed)
        {
            if (inter.CanBePingedByPlayer) {
                //door specific code, namely keys
                if (inter.GetComponent<DoorScript>())
                {
                    if (inter.GetComponent<DoorScript>().KeysNeeded <= basicKeys) {
                        inter.pinged = true;
                        basicKeys -= inter.GetComponent<DoorScript>().KeysNeeded;
                    }
                } else
                {
                    inter.pinged = true;
                }
            }
        }
    }

    //finds the nearest visible enemy, then sets lock on to said enemy.
    //nothing about being locked onto an enemy is handled here. this is just attaching and detaching the lockon.
    private void LockOntoEnemy()
    {
        if (LockPressed)
        {
            if (LockOnTarget == null)
            {

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                float closestDist = 100000;

                foreach (GameObject respawn in enemies)
                {
                    if (Vector3.Distance(respawn.transform.position, transform.position) < closestDist && !Physics.Linecast(PlayerModel.transform.position, respawn.transform.position, LM))
                    {
                        closestDist = Vector3.Distance(respawn.transform.position, transform.position);
                        LockOnTarget = respawn;
                    }
                }


                PlayerModel.rotation = RB.rotation;
            } else
            {
                LockOnTarget = null;
            }
        }
    }

    //updates the number of health shown in the hud
    //it was going to be much more complicated, but I couldnt find a satisfying way to just make it happen. 
    private void UpdateHearts()
    {
        healthGText.text = health + "";
    }

    //last minute change because of using fixedupdate
    //basically, the GetKeyPressed could trigger between frames, because the framerate would be too fast compared to fixedupdate
    //this new system relies solely on GetKey, so the pressed and released events should follow the fixedUpdate rate instead.
    private void UpdateKeys()
    {
        //There is an easy way to make this simple: make a struct that contains the key to check, pressed, prevHeld, and held
        //but Im lazy and dont remember how to make structs in C# so ill just settle for good ol copy pasting!

        BowPressed = false;
        BoomerPressed = false;
        SwordPressed = false;
        LockPressed = false;
        JumpPressed = false;
        InteractPressed = false;

        BowLastHeld = BowHeld;
        BoomerLastHeld = BoomerHeld;
        SwordLastHeld = SwordHeld;
        LockLastHeld = LockHeld;
        InteractLastHeld = InteractHeld;
        JumpLastHeld = JumpHeld;

        if (Input.GetKey(Item1) && !BowHeld)
        {
            BowPressed = true;
            BowHeld = true;
        }
        else if (!Input.GetKey(Item1))
        {
            BowHeld = false;
        }

        if (Input.GetKey(Item2) && !BoomerHeld)
        {
            BoomerPressed = true;
            BoomerHeld = true;
        }
        else if (!Input.GetKey(Item2))
        {
            BoomerHeld = false;
        }

        if (Input.GetKey(SwingSword) && !SwordHeld)
        {
            SwordPressed = true;
            SwordHeld = true;
        }
        else if (!Input.GetKey(SwingSword))
        {
            SwordHeld = false;
        }

        if (Input.GetKey(Jumping) && !JumpHeld)
        {
            JumpPressed = true;
            JumpHeld = true;
        }
        else if (!Input.GetKey(Jumping))
        {
            JumpHeld = false;
        }

        if (Input.GetKey(Interact) && !InteractHeld)
        {
            InteractPressed = true;
            InteractHeld = true;
        }
        else if (!Input.GetKey(Interact))
        {
            InteractHeld = false;
        }

        if (Input.GetKey(lockOn) && !LockHeld)
        {
            LockPressed = true;
            LockHeld = true;
        }
        else if (!Input.GetKey(lockOn))
        {
            LockHeld = false;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactible>())
        {
            inter = other.gameObject.GetComponent<Interactible>();
        }

        if (other.GetComponent<HealthPickUp>())
        {
            health = Mathf.Clamp(health + other.GetComponent<HealthPickUp>().healthGiven, 0, maxHealth);
            Destroy(other.gameObject);
        }

        if (other.GetComponent<MaxHealthPickUp>())
        {
            maxHealth = maxHealth + other.GetComponent<MaxHealthPickUp>().maxHealthGiven;
            health = maxHealth;
            Destroy(other.gameObject);
        }

        if(other.GetComponent<EnemyAttackScript>() && iAmInvincible == false)
        {
            health = health - other.GetComponent<EnemyAttackScript>().damage;
            StartCoroutine(BasicHit());
        }

        if (other.GetComponent<FlyingMonsterScript>() && iAmInvincible == false)
        {
            health = health - other.GetComponent<FlyingMonsterScript>().damage;
            StartCoroutine(BasicHit());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactible>())
        {
            inter = null;
        }
    }

    //alex made this. I, davis, prefer float based timers because I can use them to do things at certain intervals. 
    IEnumerator BasicHit()
    {
        iAmInvincible = true;
        yield return new WaitForSeconds(3);
        iAmInvincible = false;
    }
}
