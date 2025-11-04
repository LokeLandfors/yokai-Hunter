using UnityEngine;

public class EnemyTouch : MonoBehaviour
{
    public int damage = 2; //Skada som spelaren ska ta när Enemy rör honom

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerRespawn player = collision.gameObject.GetComponent<PlayerRespawn>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
