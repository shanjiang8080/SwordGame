using System.Linq;
using UnityEngine;

public class CoinCreation : MonoBehaviour
{
    public int uid;

    void Start()
    {
        GameObject sceneManager = sceneController.instance.gameObject;
        CoinCounter counter = sceneManager.GetComponent<CoinCounter>();

        uid = $"{transform.position}".GetHashCode();
        // if you have been collected previously, destroy yourself
        if (counter.coins.Contains(uid)) {
            Debug.Log("Not spawning because previously collected!");
            Destroy(gameObject);
        }
    }

}
