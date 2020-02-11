using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReGenerator : MonoBehaviour
{

    // setup raycast stuff
    int layerMask = 1 << 9; // layer 9 is the interior layer
    RaycastHit hit;
    private bool notChangedRecently = true;
    public int timeBeforeRegenAgain;

    // sets up repeating function that periodically allows the room to be re-generated again
    public void Start() {
        InvokeRepeating("allowChanges", 0, timeBeforeRegenAgain);

    }

    // sets room state to allow it to be generated again
    private void allowChanges() {
        notChangedRecently = true;
    }

    // regenerates the room at the given point
    public void regenPoint() {

        // only regenerate room if it hasn't changed recently 
        if(notChangedRecently) {
            notChangedRecently = false;
                                
            // detect and destroy interior
            if(Physics.Raycast(transform.position, transform.up, out hit, 30, layerMask)){

                // Debug.Log("Regenerating room");
            
                // destroy old interior
                Destroy(hit.collider.gameObject);
                
                // regenerate new interior
                GetComponent<RoomAdder>().AddInterior();
            
            }
        }
    }
}
