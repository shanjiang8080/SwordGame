using TMPro;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField]
    public string text;
    private TMP_Text box;
    private string beforeText;
    public void Start()
    {
        box = GetComponent<TMP_Text>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        beforeText = box.text;
        box.text = text;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        box.text = beforeText;
    }
}
