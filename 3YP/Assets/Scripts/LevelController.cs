using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject scarecrow;
    public bool scarecrowMoving = true;

    private ScarecrowController scarecrowController;

    void Start() {
        scarecrowController = scarecrow.GetComponent<ScarecrowController>();
    }
    void Update() {
        if(Input.GetKeyDown("1")) {
            scarecrowMoving = false;
            scarecrowController.stop();

        }
        
    }
}
