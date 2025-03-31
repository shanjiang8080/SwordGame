using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeCycle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // set a point in 3d space to be its current position
    public static Vector2 CurrentRespawn;
    private Collider2D collider;
    void Start()
    {
        if (CurrentRespawn == Vector2.zero)
        {
            CurrentRespawn = gameObject.transform.position;
        } else
        {
            transform.position = CurrentRespawn;
        }
        collider = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"From Player: collision from {collision.tag}");
        
        if (collision.IsTouching(collider) && collision.CompareTag("Damage"))
        {
            Die();
        }
    }

    public void Die()
    {
        // reset person at nearest checkpoint

        // play death animation
        // tbd right now, just reload the scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
