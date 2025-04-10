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
            counter.IncrementCoin();
            // get the game object and add it to the coins
            var userID = collision.gameObject.GetComponent<CoinCreation>().uid;
            counter.coins.Add(userID);

            // destroy the coin
            Destroy(collision.gameObject);
        }
    }
}
