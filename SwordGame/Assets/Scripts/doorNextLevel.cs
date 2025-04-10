using UnityEngine;

public class doorNextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) // Corrected for 2D
    {
        if (collision.CompareTag("Player")) 
        {
            // Go to the next level
            PlayerLifeCycle.CurrentRespawn = Vector2.zero;
            sceneController.instance.NextLevel();
        }
    }
}
