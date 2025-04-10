using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public static CoinUI instance;
    public static bool isThere;
    private void Awake()
    {
        if (!isThere)
        {
            isThere = true;
            instance = gameObject.transform.GetChild(0).GetComponent<CoinUI>();
            Debug.Log("Not destroying self");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("destroying self");
            Destroy(gameObject);
        }
    }
}
