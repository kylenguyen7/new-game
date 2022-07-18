using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    // If specified, destroys this parent (instead of the child, for example if this is placed on a child sprite)
    [SerializeField] private GameObject optionalParentToDestroy;
    public void DestroyMe() {
        if (optionalParentToDestroy != null) {
            Destroy(optionalParentToDestroy);
        } else {
            Destroy(gameObject);
        }
    }
}
