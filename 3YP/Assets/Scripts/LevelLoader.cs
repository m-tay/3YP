using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;


    // Update is called once per frame
    void Update()
    {
        
    }

    
    IEnumerator LoadLevel(string level) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(level);

    }

    public void loadGameOverScreen() { 

        string level = "GameOverScreen";
        StartCoroutine(LoadLevel(level));
    }

    public void loadGameWinScreen() { 

        string level = "WinScreen";        
        StartCoroutine(LoadLevel(level));
    }


}
