using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    static PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();
    }

    public override void Interact(PlayerMovement player)
    {
        Destroy(gameObject);
    }
}
