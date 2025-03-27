using UnityEngine;

public class MonsterLifecycle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            Debug.Log("Dying!!!");
            Die();
        }
    }
    public void Die()
    {
        // play death animation
        // tbd right now, just remove yourself.
        Destroy(gameObject);
    }
}
