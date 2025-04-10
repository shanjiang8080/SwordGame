using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    public int CoinCount => _coinCount;
    private int _coinCount;
    public HashSet<int> coins = new HashSet<int>();
    public TMP_Text coinText;
    private string SceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (CoinUI.instance != null)
            coinText = CoinUI.instance.GetComponent<TMP_Text>();
        _coinCount = 0;
        SceneName = gameObject.scene.name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneName)
        {
            return;
        }
        SceneName = scene.name;
        // resetting coins
        Debug.Log("Resetting coin list!");
        coins.Clear();

        if (coinText == null)
        {
            coinText = CoinUI.instance.GetComponent<TMP_Text>();
        }
        if (CoinCount != 0)
            coinText.text = $"Coins: {CoinCount}";
    }
    public void IncrementCoin()
    {
        _coinCount++;
        if (coinText != null)
        {
            coinText.text = $"Coins: {CoinCount}";
        }
    }

}
