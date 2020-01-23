using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    public LevelLoader levelLoader;

    public float speed = 10.0f;
    private float translation;
    private float strafe;
    private bool noClip = false;

    // Use this for initialization
    void Start () {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;		
	}
	
	// Update is called once per frame
	void Update () {
        // Input.GetAxis() is used to get the user's input
        // You can furthor set it on Unity. (Edit, Project Settings, Input)
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        strafe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        // no clip mode transformation
        if(!noClip) {
            transform.Translate(strafe, 0, translation); 
        }
        else {
            transform.position = transform.position + Camera.main.transform.forward * (speed * 3) * Time.deltaTime * Input.GetAxis("Vertical");
        }

        if (Input.GetKeyDown("escape")) {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }

        // toggle no clipping
        if (Input.GetKeyDown("g")) {

            if(!noClip) {
                Debug.Log("No clipping mode enabled");
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
                rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
                rb.constraints &= ~RigidbodyConstraints.FreezeRotationX; 

                Collider col = GetComponent<Collider>();
                col.enabled = false;

                noClip = true;
            }
            else {
                Debug.Log("No clipping mode disabled");
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX; 

                Collider col = GetComponent<Collider>();
                col.enabled = true;

                noClip = false;
            }

        }
    }

    // check collisions with end game object
    void OnCollisionEnter(Collision col) {
        
        if(col.gameObject.name == "EndPoint") {

            Debug.Log("EndPoint collision!");

            // transition to gameover screen
            levelLoader.GetComponent<LevelLoader>().loadGameWinScreen();


            }
    }
}