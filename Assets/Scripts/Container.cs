using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Damageable {

    // if true, a single item will be dropped from the whatToDrop list
    // if false, all items from the whatToDrop list will be dropped
    [SerializeField]
    bool givesRandom = false;

    [SerializeField]
    protected List<GameObject> whatToDrop;
    protected List<GameObject> gameObjects;


    [SerializeField]
    Vector3 goRotation;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        gameObjects = new List<GameObject>();
    }

    protected override void Die() {
        if( givesRandom ) {
            GameObject go = whatToDrop[ Random.Range( 0, whatToDrop.Count ) ];
            if( go ) {
                gameObjects.Add( Instantiate( go, transform.position, Quaternion.Euler( goRotation ) ) );
            }
        }
        else {
            foreach( GameObject go in whatToDrop ) {
                if( go ) {
                    gameObjects.Add( Instantiate( go, transform.position, Quaternion.Euler( goRotation ) ) );
                }
            }
        }

        base.Die();
    }

}
