using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private GameObject sceneManager;
    private CoinCounter counter;
    private Collider2D collider;

    public void Start()
    {
        sceneManager = sceneController.instance.gameObject;
        counter = sceneManager.GetComponent<CoinCounter>();
        collider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouching(collider) && collision.CompareTag("Coin"))
        {
            counter.CoinCount++;
            // destroy the coin
            Destroy(collision.gameObject);
        }
    }
}
