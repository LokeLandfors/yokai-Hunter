using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : Interactable
{
    static PlayerMovement player;
    public ButtonUI gameManager;
    // Start is called before the first frame update
public virtual void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();
    }
    public override void Interact(PlayerMovement player)
    {
        gameManager.Winner();
    }
}
