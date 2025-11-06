using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    // wayk
    [Header("Respawn Settings")]
    public Transform respawnPoint;
    public int playerHealth = 10;

    [Header("Death Settings")]
    public GameObject deathScreenUI; 
    [SerializeField] private GameObject[] deathParticles;
    public float deathDelay = 0.5f; 

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
        //pausa
        Time.timeScale = 0f;
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
