using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint; //Stället där Player ska respawna
    public int playerHealth = 10; // Player's health


    void Update()
    {
        if (playerHealth <= 0)
        {
            Respawn();
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log("Player Health: " + playerHealth);

        if (playerHealth <= 0)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
