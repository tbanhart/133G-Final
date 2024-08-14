using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
    [SerializeField] public string NewScene;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        
    }

    public void DoStartUp()
    {
        var currentScene = GameObject.Find(NewScene);
        var player = Instantiate(playerObject);
        player.gameObject.name = "Player";

        if (currentScene == null) { 
            player.transform.position = Vector3.zero;
        }
        else
            player.transform.position = currentScene.GetComponent<SwitchScene>().playerStartPos.transform.position;

        

        Destroy(gameObject);
    }
}
