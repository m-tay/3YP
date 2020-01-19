using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowController : MonoBehaviour
{
    public enum State {
        Wandering,
        Chasing
    }
    public GameObject player;
    public State state = State.Chasing;
    public float rotationSpeed = 1.0f;
    public float moveSpeed = 0.5f;

    private UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startNavAgent(Vector3 startPoint) {
        UnityEngine.AI.NavMeshHit closestHit;
        if( UnityEngine.AI.NavMesh.SamplePosition(startPoint, out closestHit, 500, 1 ) ){
            transform.position = closestHit.position;
            gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            Debug.Log("NavAgent for Scarecrow started successfully!");
            
        }
        else{
            Debug.Log("Error adding NavMeshAgent");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if chasing down the player
        if(state==State.Chasing) {
            // get direction
            Vector3 dir = player.transform.position - transform.position;

            // create quarternion looking at player
            //Quaternion lookRot = Quaternion.LookRotation(dir);

            // set x and z rotations to 0 (freezing rotation in rigidbody does not seem to work)
            //lookRot.x = 0; lookRot.z = 0;

            // slerp new rotation
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Mathf.Clamp01(rotationSpeed * Time.maximumDeltaTime));
     
            // move towards player (BASIC IMPLEMENTATION)
            //transform.position += transform.forward * moveSpeed * Time.deltaTime;

            // move towards player (using navmesh)
            agent.SetDestination(player.transform.position);
           
        }        
    }


}
