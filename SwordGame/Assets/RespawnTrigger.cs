using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // collision
            PlayerLifeCycle.CurrentRespawn = gameObject.transform.position;
        }
    }

}
