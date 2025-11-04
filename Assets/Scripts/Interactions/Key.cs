using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PickUp
{
    [SerializeField] GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(PlayerMovement player)
    {

        base.Interact(player);
        Destroy(door);
    }
}
