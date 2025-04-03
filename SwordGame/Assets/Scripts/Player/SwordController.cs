using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Transform sword;
    public Rigidbody2D body;
    public float smoothingSpeed = 10;
    public float throwSpeed = 20;
    public Object player;
    private Rigidbody2D playerBody;
    private Rigidbody2D swordBody;
    private float swordLength = 3;
    public float gravityScale = 3;
    public float retractVelocity = 10;
    public float retractSnapDistance = 1;
    public float retractBodyTransferRatio = 0.2f;
    public float retractInitialForce = 2f;
    public float retractMinimumVelocity = 2;

    public PhysicsMaterial2D normalMaterial;
    public PhysicsMaterial2D retractMaterial;

    private Collider2D collider;

    [Range(0f, 1f)]
    public float throwAngleDampenRatio = 0.5f;
    [Range(0f, 1f)]
    public float retractAngleDampenRatio = 0.5f;
    [Range(0f, 1f)]
    public float retractEaseForceRatio = 0.5f;



    private Quaternion oldRotation;
    private Vector2 angleV;

    public bool isBeingHeld => !isThrown && !isRetracting;
    [HideInInspector]
    public bool isThrown;
    [HideInInspector]
    public bool isRetracting;
    private bool touchingTerrain; // this is for when it's detached from the player.
    private float retractTimer = 0f;
    private float retractTimerMax = 1;

    private readonly float[] lockAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };



    private void Start()
    {
        playerBody = player.GetComponent<Rigidbody2D>();
        oldRotation = sword.transform.rotation;
        touchingTerrain = false;
        isThrown = false;
        collider = GetComponent<Collider2D>();
    }

    // the player doesn't hold the sword at the beginning if this function is called at the start
    public void StartNotHeld()
    {
        swordBody = gameObject.AddComponent<Rigidbody2D>();
        isThrown = true;
        var angle = sword.transform.eulerAngles.z;
        angle = (angle + 360) % 360;
        Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        swordBody.bodyType = RigidbodyType2D.Kinematic;
        swordBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Terrain"))
        {
            return;
        }
        if (isThrown)
        {
            touchingTerrain = true;

        }
        if (touchingTerrain && isThrown)
        {
            if (swordBody.bodyType != RigidbodyType2D.Kinematic)
            {
                Debug.Log("Locking position!");
                // lock position
                var angle = sword.transform.eulerAngles.z;
                angle = (angle + 360) % 360;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                swordBody.bodyType = RigidbodyType2D.Kinematic;
                // get the velocity, put yourself deeper in a bit.
                var something = new Vector3(dir.y, dir.x, 0).normalized * 0.2f;
                sword.transform.position += new Vector3(-something.x, something.y, 0);
                // then, lock x and y position
                swordBody.constraints = RigidbodyConstraints2D.FreezeAll;
                // trying to do snapping so it doesnt look weird but it's not working right now. i'm going on break.
                /*
                float closestAngle = lockAngles[0];
                float smallestDifference = Mathf.Abs(angle - closestAngle);

                foreach (float ang in lockAngles)
                {
                    float difference = Mathf.Abs(angle - ang);
                    if (difference < smallestDifference)
                    {
                        closestAngle = ang;
                        smallestDifference = difference;
                    }
                }

                // Set the sword’s rotation to the closest angle
                Vector2 pivotOffset = new Vector2(0, swordLength); // Example: swordLength is the distance

                // Calculate the rotated offset
                Quaternion rotation = Quaternion.Euler(0, 0, angle); // Use your calculated angle
                Vector2 rotatedOffset = rotation * pivotOffset;

                // Update the sword's position (tip remains fixed)
                Vector2 tipPosition = sword.transform.position + new Vector3(dir.x, dir.y, 0).normalized; // Replace with your tip's actual position
                sword.transform.position = tipPosition - rotatedOffset;

                Debug.Log($"angle is {angle}, snapped to {closestAngle}");
                sword.transform.rotation = Quaternion.Euler(0, 0, closestAngle);
                */



                // in the future, make yourself a parent of the colliding body, so platforms work and stuff.

            }
        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {
    }
    public void Move(Vector2 angleV)
    {
        this.angleV = angleV.normalized;
        oldRotation = sword.transform.rotation;
        // do a linear interpolate between the existing angle and the new angle.
        if (isBeingHeld)
        {
            float angle = Mathf.Atan2(angleV.y, angleV.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            sword.transform.rotation = Quaternion.Lerp(oldRotation, targetRotation, 1 / smoothingSpeed);
        }

    }
    public void FixedUpdate()
    {
        // if the physicsbody 2d isn't null, then it's either in the air, being thrown, or on the ground.
        if (isThrown && !touchingTerrain)
        {
            // make the angle of the sword the current angle.
            // get current velocity
            var vel = swordBody.linearVelocity;
            //Debug.Log(vel);
            if (vel != Vector2.zero)
            {
                var angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg - 90;
                var targetAngle = Quaternion.Euler(0, 0, angle);
                sword.transform.rotation = Quaternion.Lerp(sword.transform.rotation, targetAngle, throwAngleDampenRatio);

            }
        }
        if (isRetracting)
        {
            Vector2 distance = (playerBody.transform.position - sword.transform.position);
            if (distance.magnitude < retractSnapDistance)
            {
                Catch();
                return;
            }

            Vector2 direction = distance.normalized;
            //Debug.Log($"direction is {direction}");
            var maxVelocity = retractVelocity * retractTimer;
            if (swordBody.linearVelocity.magnitude < retractMinimumVelocity)
            {
                // give it a force in the direction
                swordBody.AddForce(direction * retractMinimumVelocity, ForceMode2D.Impulse);
            }
            else if (swordBody.linearVelocity.magnitude < maxVelocity)
            {
                swordBody.linearVelocity = Vector2.Lerp(swordBody.linearVelocity, maxVelocity * direction, retractEaseForceRatio);
            }

            // oh yeah, lerp the angle
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            var targetAngle = Quaternion.Euler(0, 0, angle);
            sword.transform.rotation = Quaternion.Lerp(sword.transform.rotation, targetAngle, retractAngleDampenRatio);
            retractTimer = Mathf.Min(retractTimer + Time.deltaTime, retractTimerMax);
        }

    }
    public void Throw()
    {
        // add a rigidBody2D component.
        swordBody = gameObject.AddComponent<Rigidbody2D>();
        swordBody.gravityScale = gravityScale;
        swordBody.AddForce(angleV * throwSpeed);
        isThrown = true;
    }
    public void Recall()
    {
        // first, unstick the sword.
        var angle = sword.transform.eulerAngles.z;
        angle = (angle + 360) % 360;
        Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        sword.transform.position -= new Vector3(dir.y, dir.x, 0).normalized * 0.3f;

        swordBody.constraints = RigidbodyConstraints2D.None;

        // set the rigidBody2D mode back to dynamic
        swordBody.bodyType = RigidbodyType2D.Dynamic;
        // disable gravity
        swordBody.gravityScale = 0;

        // add a force to forcefully unseat the sword
        swordBody.AddForce(dir.normalized * retractInitialForce, ForceMode2D.Impulse);

        // change the physics material to slippery
        collider.sharedMaterial = retractMaterial;

        // set isRetracting to true
        isRetracting = true;
        // set isThrown to false
        isThrown = false;
        retractTimer = 0f;
    }
    public void Catch()
    {
        Debug.Log("Caught");
        // after recall, distance is less than a bit.
        // take the existing velocity
        var velocity = swordBody.linearVelocity;
        // now disable the physics...
        swordBody.simulated = false;

        // parent it, then snap to player
        sword.transform.SetParent(playerBody.transform, false);
        sword.transform.localPosition = Vector3.zero;
        // give the player a force 
        playerBody.AddForce(velocity * retractBodyTransferRatio, ForceMode2D.Impulse);
        // delete the rigidBody2D
        Destroy(swordBody);

        // make material normal
        collider.sharedMaterial = normalMaterial;

        // reset bools
        isThrown = false;
        isRetracting = false;
        touchingTerrain = false;
        // set parent of player
    }

}
