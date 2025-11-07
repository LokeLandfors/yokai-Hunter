using UnityEngine;

public class Orb : PickUp
{
    public override void Interact(PlayerMovement player)
    {
        base.Interact(player);

     /*  if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoin(1);
        }*/
    }
}
