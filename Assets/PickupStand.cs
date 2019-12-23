using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStand : ItemStand
{

    protected override void SpawnItem() {
        Pickup pickupToSpawn = null;

        foreach( Pickup pickup in itemPool ) {
            if( !pickup.gameObject.activeInHierarchy ) {
                pickupToSpawn = pickup;
                break;
            }
        }

        if( pickupToSpawn ) {
            pickupToSpawn.Enable();
            pickupToSpawn.transform.position = transform.position + itemOffset;
            hasItem = true;
        }
        else {
            hasItem = false;
        }
    }

}
