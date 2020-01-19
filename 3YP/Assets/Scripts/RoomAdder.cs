using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoomAdder : MonoBehaviour
{
    public GameObject[] tiles;  // holds tiles that can be used to fill map
    public GameObject[] interiors;
    public GameObject[] doors;

    public GameObject target; 

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

    public void AddDoors() {
        // DOOR SPAWNING

        // setup raycasting
        RaycastHit hit;
        Ray ray = new Ray();    
        ray.origin = transform.position;
        
        // setup layer mask for doors
        int layermask = 1 << 11;

        // // setup layer mask for doors when they spawned
        // int layermaskSpawned = 1 << 12;

        // forward check        
        if(Physics.Raycast(transform.position, Vector3.forward, out hit, 20, layermask)) {
            target = hit.transform.gameObject;
            Instantiate(doors[0], target.transform.position, Quaternion.identity, target.transform);
            target.layer = 12;
        }    

        // backwards check        
        if(Physics.Raycast(transform.position, Vector3.back, out hit, 20, layermask)) {
            target = hit.transform.gameObject;
            Instantiate(doors[0], target.transform.position, Quaternion.identity, target.transform);
            target.layer = 12;
        }    

        // left check        
        var rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);
        if(Physics.Raycast(transform.position, Vector3.left, out hit, 20, layermask)) {
            target = hit.transform.gameObject;
            Instantiate(doors[0], target.transform.position, Quaternion.identity * rotation, target.transform);
            target.layer = 12;
        }            
         
        // right check        
        if(Physics.Raycast(transform.position, Vector3.right, out hit, 20, layermask)) {
            target = hit.transform.gameObject;
            Instantiate(doors[0], target.transform.position, Quaternion.identity * rotation, target.transform);
            target.layer = 12;
        }       

    }

}
