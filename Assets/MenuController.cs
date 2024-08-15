using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject sceneLoader;
    [SerializeField] string targetScene;

    public void StartGame()
    {
        var obj = Instantiate(sceneLoader);

        obj.GetComponent<SceneTransition>().NewScene = targetScene;
        obj.name = "Scene Transition";

        SceneManager.LoadScene("Area_Backyard");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
