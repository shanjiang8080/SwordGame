using Unity.VisualScripting;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Transform sword;
    public Rigidbody2D body;
    public float smoothingSpeed = 10;
    public Object player;
    private Rigidbody2D playerBody;

    private bool isColliding = false;

    private void Start()
    {
        playerBody = player.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding");
        foreach (var collide in collision.contacts)
        {
            Vector2 collisionNormal = collide.normal;
            Vector2 flingDirection = Vector2.Reflect(body.linearVelocity, collisionNormal);

            playerBody.AddForce(flingDirection * body.linearVelocity.magnitude, ForceMode2D.Impulse);
            
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }
    public void Move(Vector2 angleV)
    {
        // do a linear interpolate between the existing angle and the new angle.
        float angle = Mathf.Atan2(angleV.y, angleV.x) * Mathf.Rad2Deg - 90;
        float oldAngle = sword.localEulerAngles.z;
        float smoothedAngle = Mathf.LerpAngle(oldAngle, angle, 0.5f);
        body.rotation = smoothedAngle;
        sword.localPosition = Vector3.zero;
    }

}
