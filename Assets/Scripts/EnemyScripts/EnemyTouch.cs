using UnityEngine;

public class EnemyTouch : MonoBehaviour
{
    public int damage = 2; //Skada som spelaren ska ta när Enemy rör honom

    void OnTriggerEnter2D(Collision2D trigger)
    {
        if (trigger.gameObject.CompareTag("Player"))
        {
            PlayerRespawn player = trigger.gameObject.GetComponent<PlayerRespawn>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
