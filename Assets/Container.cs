using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Damageable {

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
        foreach( GameObject go in whatToDrop ) {
            gameObjects.Add( Instantiate( go, transform.position, Quaternion.Euler( goRotation ) ) );
        }
        base.Die();
    }

}
