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

    private Quaternion oldRotation;
    private Vector2 angleV;

    private bool isThrown;
    private bool touchingTerrain; // this is for when it's detached from the player.
    private bool _touch;

    private readonly float[] lockAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };



    private void Start()
    {
        playerBody = player.GetComponent<Rigidbody2D>();
        oldRotation = sword.transform.rotation;
        touchingTerrain = false;
        isThrown = false;
        _touch = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isThrown)
        {
            //touchingTerrain = true;
            if (_touch)
            {
                touchingTerrain = true;
            } else
            {
                _touch = true;
            }
        }
        if (touchingTerrain)
        {
            if (swordBody.bodyType != RigidbodyType2D.Static)
            {
                // lock position
                var angle = sword.transform.eulerAngles.z;
                angle = (angle + 360) % 360;
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                swordBody.bodyType = RigidbodyType2D.Static;
                // get the velocity, put yourself deeper in a bit.
                sword.transform.position += new Vector3(dir.y, dir.x, 0).normalized * 0.2f;
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
        float angle = Mathf.Atan2(angleV.y, angleV.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        Quaternion deltaRotation = Quaternion.Inverse(oldRotation) * targetRotation;
        sword.transform.rotation = Quaternion.Euler(0, 0, angle);
        
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
                sword.transform.rotation = Quaternion.Euler(0, 0, angle);

            }
        }

    }
    public void Throw()
    {
        // add a rigidBody2D component.
        swordBody = gameObject.AddComponent<Rigidbody2D>();
        swordBody.gravityScale = 3;
        swordBody.AddForce(angleV * throwSpeed);
        isThrown = true;
    }

}
