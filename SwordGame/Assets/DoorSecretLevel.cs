using UnityEngine;

public class DoorSecretLevel : MonoBehaviour
{
    public int levelID;
    private void OnTriggerEnter2D(Collider2D collision) // Corrected for 2D
    {
        if (collision.CompareTag("Player"))
        {
            // Go to the next level
            PlayerLifeCycle.CurrentRespawn = Vector2.zero;
            sceneController.instance.GoToLevel(levelID);
        }
    }
}
