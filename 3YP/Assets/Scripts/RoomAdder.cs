using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoomAdder : MonoBehaviour
{
    public GameObject[] tiles;  // holds tiles that can be used to fill map
    public GameObject[] interiors;
    public void CheckAndFill() {

        // create a up vector
        Vector3 up = transform.TransformDirection(Vector3.up);

        // cast a ray up to see if rooms are detected
        if(Physics.Raycast(transform.position, up, 10)) {
            Debug.Log("ROOM DETECTED AT" + transform.position);
        }
        else {
            // if no room detected, spawn a room
            Debug.Log("ROOM NOT DETECTED AT" + transform.position);
            Instantiate(tiles[0], transform.position, Quaternion.identity);
            
        }

        // generate random interior (this happens in all rooms)
        int r = Random.Range(0, interiors.Length);
        Instantiate(interiors[r], transform.position, Quaternion.identity);

        
    }
}
