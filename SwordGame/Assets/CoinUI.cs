using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public static CoinUI instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.transform.GetChild(0).GetComponent<CoinUI>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
