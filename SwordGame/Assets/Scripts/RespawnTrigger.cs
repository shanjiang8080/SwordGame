using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public Sprite litSprite;
    public SpriteRenderer SpriteRenderer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // collision
            PlayerLifeCycle.CurrentRespawn = gameObject.transform.position;
            SpriteRenderer.sprite = litSprite;
        }
    }

}
