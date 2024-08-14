using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
    [SerializeField] public string NewScene;
    Dictionary<ItemType, int> playerInventory;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        if(GameObject.Find("Player") == null) playerInventory = new Dictionary<ItemType, int>();
        else playerInventory = GameObject.Find("Player").GetComponent<PlayerController>().Inventory;
    }

    public void DoStartUp()
    {
        var currentScene = GameObject.Find(NewScene);
        var player = Instantiate(playerObject);
        var playerController = player.GetComponent<PlayerController>();
        player.gameObject.name = "Player";
        if(playerInventory == new Dictionary<ItemType, int>())
            playerController.Inventory = new Dictionary<ItemType, int> {
            {ItemType.LEAF, 0},
            {ItemType.ROCK, 0},
            {ItemType.FLAG, 0}
        };
        else playerController.GetComponent<PlayerController>().Inventory = playerInventory;

        if (currentScene == null) { 
            player.transform.position = Vector3.zero;
        }
        else
            player.transform.position = currentScene.GetComponent<SwitchScene>().playerStartPos.transform.position;

        

        Destroy(gameObject);
    }
}
