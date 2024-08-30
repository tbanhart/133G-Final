using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] GameObject playerObject;
    [SerializeField] public string NewScene;
    [SerializeField] public string OldScene;
    Dictionary<ItemType, int> playerInventory;
    
    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        if(GameObject.FindWithTag("Player") == null) playerInventory = new Dictionary<ItemType, int>();
        else playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerController>().Inventory;
    }

    public void DoStartUp()
    {
        var currentScene = GameObject.Find(OldScene);
        Debug.Log(OldScene);
        Debug.Log(currentScene);
        var player = Instantiate(playerObject);
        var playerController = player.GetComponent<PlayerController>();
        player.gameObject.name = "Player";
        if (playerInventory == new Dictionary<ItemType, int>())
            playerController.Inventory = new Dictionary<ItemType, int> {
            {ItemType.LEAF, 0},
            {ItemType.ROCK, 0},
            {ItemType.FLAG, 0}
        };
        else playerController.GetComponent<PlayerController>().Inventory = playerInventory;
        foreach (var item in playerInventory) {
            playerController.UpdateItemAmounts(item.Key, item.Value);
        }

        if (currentScene == null) {
            player.transform.position = Vector3.zero;
        }
        else { 
            Debug.Log(currentScene.GetComponent<SwitchScene>().playerStartPos.transform.position);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = currentScene.GetComponent<SwitchScene>().playerStartPos.transform.position;
            player.GetComponent<CharacterController>().enabled = true;
        }
        

        Destroy(gameObject);
    }
}
