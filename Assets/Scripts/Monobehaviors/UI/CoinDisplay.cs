using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    TextMeshProUGUI coinText;

    private void Awake()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }

    public void SetCoinText(int value)
    {
        coinText.text = value.ToString();
    }
}
