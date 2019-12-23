using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    [Range( 0.0f, 100.0f ), SerializeField]
    float healAmount = 0f;

    protected override void Collect( GameObject collectingObject ) {
        PlayerDamageable player = collectingObject.GetComponent<PlayerDamageable>();

        player.Heal( healAmount );

        base.Collect( collectingObject );
    }

}
