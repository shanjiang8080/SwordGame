using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // the speed the player moves left/right
    public float moveSpeed = 20f;
    // the power of the jump.
    public float jumpHeight = 5f;
    
    // internal value for the direction of movement.
    // has both a direction and a strength.
    private float moveDirection;

    void Update()
    {
        ProcessInputs();
        Move();
    }
    void ProcessInputs()
    {
        moveDirection = Input.GetAxis("Horizontal");
    }
    void Move()
    {
        transform.position += new Vector3(moveDirection, 0, 0) * moveSpeed * Time.deltaTime;
    }
}
