using UnityEngine;
using UnityEngine.UI;

public class OrbManager : MonoBehaviour
{
    public static OrbManager Instance;

    [SerializeField] private Text coinText;
    private int coinCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateCoinUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coinCount.ToString();
    }
}
