using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform respawnPoint;
    public int playerHealth = 10;

    [Header("Death Settings")]
    public GameObject deathScreenUI;
    [SerializeField] private GameObject[] deathParticles;
    public float deathDelay = 0.5f;

    [Header("Blood Animation")]
    public RectTransform blood2; 
    public float bloodAnimDuration = 5f; 
    private Vector2 bloodStartPos = new Vector2(0, 850);
    private Vector2 bloodEndPos = new Vector2(0, 300);

    private bool isDead = false;

    void Update()
    {
        if (playerHealth <= 0 && !isDead)
        {
            StartCoroutine(HandleDeath());
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        playerHealth -= damage;
        Debug.Log("Player Health: " + playerHealth);

        if (playerHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        isDead = true;

        foreach (var particle in deathParticles)
        {
            if (particle != null)
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }
        }

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;

        yield return new WaitForSeconds(deathDelay);


        if (deathScreenUI != null)
            deathScreenUI.SetActive(true);

     
        if (blood2 != null)
        {
            blood2.anchoredPosition = bloodStartPos;
            StartCoroutine(AnimateBlood());
        }

        
        Time.timeScale = 0f;
    }

    private IEnumerator AnimateBlood()
    {
        float elapsed = 0f;
        while (elapsed < bloodAnimDuration)
        {
            elapsed += Time.unscaledDeltaTime; 
            float t = elapsed / bloodAnimDuration;
            blood2.anchoredPosition = Vector2.Lerp(bloodStartPos, bloodEndPos, t);
            yield return null;
        }
        blood2.anchoredPosition = bloodEndPos; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
