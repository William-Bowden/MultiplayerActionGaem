using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrabber : MonoBehaviour {
    List<Interactable> interactables;

    bool GAMERUNNING = false;

    public Transform weaponHeld = null;

    // Start is called before the first frame update
    void Awake() {
        interactables = new List<Interactable>();

        GAMERUNNING = true;
    }

    public Gun Interact() {
        Interactable closestInteractable = null;
        float shortestDistance = 0.0f;

        foreach( Interactable interactable in interactables ) {
            WeaponPickup weapon = interactable.GetComponent<WeaponPickup>();

            float dist = Mathf.Abs( ( transform.parent.position - interactable.transform.position ).magnitude );

            if( weapon ) {
                // if this weapon is not available or if it's too far, skip checking it
                if( !weapon.IsAvailable() || dist > weapon.pickupRadius ) {
                    continue;
                }

                // if already holding a weapon, closest interactable isn't a weapon, but the current is
                if( weaponHeld && !closestInteractable.GetComponent<WeaponPickup>() ) {
                    continue;
                }
            }

            // if there is no current interactable or the distance to this interactable
            // is shorter than the current closest interactable
            if( !closestInteractable || shortestDistance > dist ) {
                // set this interactable as the new closest
                closestInteractable = interactable;
                shortestDistance = dist;
            }
        }

        // if there is an interactable that is the closest and within reach
        if( closestInteractable ) {
            closestInteractable.Interact();

            WeaponPickup pickup = closestInteractable.GetComponent<WeaponPickup>();
            if( pickup ) {
                pickup.Interact( weaponHeld, transform );
                weaponHeld = pickup.transform;
                Debug.Log( "Weapon Picked Up" );
                //return pickup.GetComponent<Gun>();
            }

        }
        else if( weaponHeld ) {
            WeaponPickup pickup = weaponHeld.GetComponent<WeaponPickup>();
            pickup.SetAvailability( true );
            weaponHeld = null;
        }

        return weaponHeld.GetComponent<Gun>();
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        interactables.Add( collision.GetComponent<Interactable>() );
    }
    private void OnTriggerExit2D( Collider2D collision ) {
        interactables.Remove( collision.GetComponent<Interactable>() );
    }

    void OnDrawGizmos() {
        if( GAMERUNNING && interactables.Count > 0 ) {
            // draw a line to all weapons
            foreach( Interactable interactable in interactables ) {
                if( interactable == null ) {
                    continue;
                }

                WeaponPickup weapon = interactable.GetComponent<WeaponPickup>();

                if( weapon ) {
                    // set the line color to red
                    Gizmos.color = Color.red;

                    float dist = Mathf.Abs( ( transform.parent.position - interactable.transform.position ).magnitude );

                    // set the line color to green for weapons in reach
                    if( dist <= weapon.pickupRadius ) {
                        Gizmos.color = Color.green;
                    }
                }

                Gizmos.DrawLine( transform.parent.position, interactable.transform.position );
            }
        }
    }

}
