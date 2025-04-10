using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinCounter : MonoBehaviour
{
    public float CoinCount;
    public HashSet<int> coins = new HashSet<int>();
    private string SceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CoinCount = 0;
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
    }
}
