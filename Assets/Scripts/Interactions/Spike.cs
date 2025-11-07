using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Spike : Interactable
{
    public static ResetManager reseter;

    private void Start()
    {
        reseter = Object.FindFirstObjectByType<ResetManager>();
    }
    public override void Interact(PlayerMovement player)
    {

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        base.Interact(player);

        if (DeathManager.Instance != null)
        {
            DeathManager.Instance.AddDeath(1);
        }
        reseter.ResetScene();
    }
}
