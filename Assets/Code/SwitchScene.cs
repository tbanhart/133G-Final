using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SwitchScene : MonoBehaviour
{
    [SerializeField] public string targetScene;
    [SerializeField] public GameObject playerStartPos;
    [SerializeField] public GameObject transitionScene;

    private void Awake()
    {
        //GameObject.Find("Player").transform.position = playerStartPos.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {

        GameObject collisionObject = other.gameObject;
        while(gameObject.transform.parent != null)
        {
            collisionObject = gameObject.transform.parent.gameObject;
        }
        if (collisionObject.name != "Player") return;

        //collisionObject.transform.position = Vector3.zero;
        //collisionObject.GetComponent<PlayerController>().FreezeObject = true;

        //collisionObject.GetComponent<PlayerController>().CurrentScene = targetScene;

        var transition = Instantiate(transitionScene);
        transition.GetComponent<SceneTransition>().NewScene = targetScene;
        transition.name = "Scene Transition";

        SceneManager.LoadScene(targetScene);
    }
}
