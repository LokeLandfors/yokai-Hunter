using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : Interactable
{
    public bool hasCheckpoint = false;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public override void Interact(PlayerMovement player)
    {
        
        {
            player.respawnPoint = transform.position;
            print("HasCheck");
            hasCheckpoint = true;
            Color newColor = new Color(0, 255, 0);
                spriteRenderer.color = newColor;
        }
    }

}

