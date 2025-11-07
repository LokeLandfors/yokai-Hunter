using UnityEngine;
using UnityEngine.UI;
public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance;

    [SerializeField] private Text deathText;
    private int deathCount = 0;

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

        UpdateDeathUI();
    }

    public void AddDeath(int amount)
    {
        deathCount += amount;
        UpdateDeathUI();
    }

    private void UpdateDeathUI()
    {
        if (deathText != null)
            deathText.text = "Deaths: " + deathCount.ToString();
    }
}
